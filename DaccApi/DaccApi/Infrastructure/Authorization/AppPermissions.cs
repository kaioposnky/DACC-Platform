namespace DaccApi.Infrastructure.Authentication
{
    public static class AppPermissions
    {
        public static class Users
        {
            public const string View = "users.view";
            public const string ViewAll = "users.viewall";
            public const string Create = "users.create";
            public const string Update = "users.update";
            public const string Delete = "users.delete";
            public const string RefreshToken = "users.refreshtoken";
            public const string Logout = "users.logout";
        }

        public static class Noticias
        {
            public const string View = "noticias.view";
            public const string Create = "noticias.create";
            public const string Update = "noticias.update";
            public const string Delete = "noticias.delete";
        }

        public static class Projetos
        {
            public const string View = "projetos.view";
            public const string Create = "projetos.create";
            public const string Update = "projetos.update";
            public const string Delete = "projetos.delete";
            public const string AddMembers = "projetos.members.add";
            public const string RemoveMembers = "projetos.members.remove";
        }

        public static class Produtos
        {
            public const string View = "produtos.view";
            public const string Create = "produtos.create";
            public const string Update = "produtos.update";
            public const string Delete = "produtos.delete";
        }

        public static class Eventos
        {
            public const string View = "eventos.view";
            public const string Create = "eventos.create";
            public const string Update = "eventos.update";
            public const string Delete = "eventos.delete";
            public const string Register = "eventos.register";
        }

        public static class Forum
        {
            public const string ViewPosts = "forum.posts.view";
            public const string CreatePost = "forum.posts.create";
            public const string UpdateOwnPost = "forum.posts.update";
            public const string DeleteOwnPost = "forum.posts.delete";
            public const string VoteOnPost = "forum.posts.vote";
            public const string CreateComment = "forum.comments.create";
            public const string UpdateOwnComment = "forum.comments.update";
            public const string DeleteOwnComment = "forum.comments.delete";
            public const string VoteOnComment = "forum.comments.vote";
            public const string AcceptComment = "forum.comments.accept";
            
            public static class Admin
            {
                public const string UpdateAnyPost = "forum.admin.posts.update";
                public const string DeleteAnyPost = "forum.admin.posts.delete";
                public const string UpdateAnyComment = "forum.admin.comments.update";
                public const string DeleteAnyComment = "forum.admin.comments.delete";
            }
        }

        public static class Faculty
        {
            public const string View = "faculty.view";
            public const string Create = "faculty.create";
            public const string Update = "faculty.update";
            public const string Delete = "faculty.delete";
        }

        public static class Cart
        {
            public const string View = "cart.view";
            public const string AddItem = "cart.items.add";
            public const string UpdateItem = "cart.items.update";
            public const string RemoveItem = "cart.items.remove";
            public const string Clear = "cart.clear";
        }

        public static class Reviews
        {
            public const string View = "reviews.view";
            public const string Create = "reviews.create";
            public const string Update = "reviews.update";
            public const string Delete = "reviews.delete";
        }
    }
}