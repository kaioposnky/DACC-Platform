import { PostCard } from '@/components/molecules';
import { Post } from '@/types';

interface PostListProps {
  posts: Post[];
  onEditPost?: (post: Post) => void;
  onDeletePost?: (postId: number) => void;
  onPostClick?: (post: Post) => void;
  loading?: boolean;
  emptyMessage?: string;
}

export const PostList = ({
  posts,
  onEditPost,
  onDeletePost,
  onPostClick,
  loading = false,
  emptyMessage = 'No posts found',
}: PostListProps) => {
  if (loading) {
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {[...Array(6)].map((_, index) => (
          <div
            key={index}
            className="h-48 bg-gray-200 rounded-lg animate-pulse"
          />
        ))}
      </div>
    );
  }

  if (posts.length === 0) {
    return (
      <div className="text-center py-12">
        <div className="text-gray-500 text-lg">{emptyMessage}</div>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {posts.map((post) => (
        <PostCard
          key={post.id}
          post={post}
          onEdit={onEditPost}
          onDelete={onDeletePost}
          onClick={onPostClick}
        />
      ))}
    </div>
  );
}; 