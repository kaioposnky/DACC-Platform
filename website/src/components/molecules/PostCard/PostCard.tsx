import { Card, Button } from '@/components/atoms';
import { Post } from '@/types';

interface PostCardProps {
  post: Post;
  onEdit?: (post: Post) => void;
  onDelete?: (postId: number) => void;
  onClick?: (post: Post) => void;
}

export const PostCard = ({ post, onEdit, onDelete, onClick }: PostCardProps) => {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  };

  return (
    <Card 
      variant="elevated" 
      className="hover:shadow-lg transition-shadow duration-200"
      onClick={() => onClick?.(post)}
    >
      <div className="space-y-3">
        <div>
          <h3 className="text-lg font-semibold text-gray-900 line-clamp-2">
            {post.title}
          </h3>
          <p className="text-gray-600 text-sm mt-1">
            {formatDate(post.createdAt)}
          </p>
        </div>
        
        <p className="text-gray-700 line-clamp-3">
          {post.content}
        </p>
        
        <div className="flex flex-wrap gap-2">
          {post.tags.map((tag) => (
            <span
              key={tag}
              className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800"
            >
              {tag}
            </span>
          ))}
        </div>
        
        {(onEdit || onDelete) && (
          <div className="flex gap-2 pt-2 border-t border-gray-200">
            {onEdit && (
              <Button 
                variant="ghost" 
                size="sm"
                onClick={(e) => {
                  e.stopPropagation();
                  onEdit(post);
                }}
              >
                Edit
              </Button>
            )}
            {onDelete && (
              <Button 
                variant="danger" 
                size="sm"
                onClick={(e) => {
                  e.stopPropagation();
                  onDelete(post.id);
                }}
              >
                Delete
              </Button>
            )}
          </div>
        )}
      </div>
    </Card>
  );
}; 