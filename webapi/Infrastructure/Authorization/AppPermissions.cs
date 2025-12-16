namespace DaccApi.Infrastructure.Authentication
{
    /// <summary>
    /// Define as constantes de permissão usadas em toda a aplicação.
    /// </summary>
    public static class AppPermissions
    {
        /// <summary>
        /// Permissões relacionadas a usuários.
        /// </summary>
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

        /// <summary>
        /// Permissões relacionadas a notícias.
        /// </summary>
        public static class Noticias
        {
            public const string View = "noticias.view";
            public const string Create = "noticias.create";
            public const string Update = "noticias.update";
            public const string Delete = "noticias.delete";
        }

        /// <summary>
        /// Permissões relacionadas a projetos.
        /// </summary>
        public static class Projetos
        {
            public const string View = "projetos.view";
            public const string Create = "projetos.create";
            public const string Update = "projetos.update";
            public const string Delete = "projetos.delete";
            public const string AddMembers = "projetos.members.add";
            public const string RemoveMembers = "projetos.members.remove";
        }

        /// <summary>
        /// Permissões relacionadas a produtos.
        /// </summary>
        public static class Produtos
        {
            public const string View = "produtos.view";
            public const string Create = "produtos.create";
            public const string Update = "produtos.update";
            public const string Delete = "produtos.delete";
        }

        /// <summary>
        /// Permissões relacionadas a eventos.
        /// </summary>
        public static class Eventos
        {
            public const string View = "eventos.view";
            public const string Create = "eventos.create";
            public const string Update = "eventos.update";
            public const string Delete = "eventos.delete";
            public const string Register = "eventos.register";
        }

        /// <summary>
        /// Permissões relacionadas ao fórum.
        /// </summary>
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
            
            /// <summary>
            /// Permissões de administrador para o fórum.
            /// </summary>
            public static class Admin
            {
                public const string UpdateAnyPost = "forum.admin.posts.update";
                public const string DeleteAnyPost = "forum.admin.posts.delete";
                public const string UpdateAnyComment = "forum.admin.comments.update";
                public const string DeleteAnyComment = "forum.admin.comments.delete";
            }
        }

        /// <summary>
        /// Permissões relacionadas a diretores (faculty).
        /// </summary>
        public static class Faculty
        {
            public const string View = "faculty.view";
            public const string Create = "faculty.create";
            public const string Update = "faculty.update";
            public const string Delete = "faculty.delete";
        }

        /// <summary>
        /// Permissões relacionadas ao carrinho de compras.
        /// </summary>
        public static class Cart
        {
            public const string View = "cart.view";
            public const string AddItem = "cart.items.add";
            public const string UpdateItem = "cart.items.update";
            public const string RemoveItem = "cart.items.remove";
            public const string Clear = "cart.clear";
        }

        /// <summary>
        /// Permissões relacionadas a avaliações (reviews).
        /// </summary>
        public static class Reviews
        {
            public const string View = "reviews.view";
            public const string Create = "reviews.create";
            public const string Update = "reviews.update";
            public const string Delete = "reviews.delete";
        }
    }
}