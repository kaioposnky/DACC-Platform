# DACC-Platform

Uma plataforma moderna e integrada para a comunidade acadêmica, composta por um **Backend robusto em .NET 8** e um **Frontend dinâmico em Next.js**.

## 🚀 Funcionalidades Principais

-   **Gestão de Conteúdo:** Sistema completo para criação e gerenciamento de Notícias, Eventos e Projetos.
-   **E-commerce Acadêmico:** Loja integrada com gestão de produtos, estoque e **pagamentos via Mercado Pago**.
-   **Portal da Comunidade:** Fórum de discussão e perfis de usuários com histórico de atividades.
-   **Painel Administrativo:** Interface intuitiva para controle total de usuários, permissões e conteúdos da plataforma.
-   **Autenticação Segura:** Sistema baseado em JWT com controle granular de permissões e recuperação de senha.

## 🏗️ Arquitetura do Projeto

O repositório segue uma estrutura de **Monorepo**:

-   **`webapi/`**: Servidor Backend robusto.
    -   **Framework:** ASP.NET Core (.NET 8)
    -   **Banco de Dados:** PostgreSQL
    -   **ORM:** Dapper + EF Core
    -   **Arquitetura:** Layered Architecture (Controller -> Service -> Repository)
-   **`website/`**: Aplicação Frontend moderna.
    -   **Framework:** Next.js 15 (App Router)
    -   **Estilização:** Tailwind CSS (Atomic Design)
    -   **Linguagem:** TypeScript

## 📚 Estrutura do Frontend (Atomic Design)

```
website/src/
├── app/                    # Diretório de rotas do Next.js
├── components/             # Componentes seguindo Atomic Design
│   ├── atoms/             # Átomos (Botões, Inputs, Typography)
│   ├── molecules/         # Moléculas (SearchBar, NewsCard)
│   ├── organisms/         # Organismos (Forms, Navigation, Footer)
│   └── templates/         # Templates de página
├── context/               # Gerenciamento de estado global (Auth, etc.)
├── services/              # Camada de integração com API (.NET)
├── types/                 # Definições de tipos TypeScript
└── utils/                 # Funções utilitárias e formatadores
```

## 🛠️ Começando

### Pré-requisitos
- .NET 8 SDK
- Node.js 18+
- PostgreSQL (ou Docker para rodar as instâncias necessárias)

### Instalação e Execução

1.  **Backend (`webapi`)**:
    ```bash
    cd webapi
    dotnet restore
    dotnet run
    ```

2.  **Frontend (`website`)**:
    ```bash
    cd website
    npm install
    npm run dev
    ```

## 📝 Scripts Disponíveis (Frontend)

- `npm run dev` - Inicia o servidor de desenvolvimento do Next.js
- `npm run build` - Gera a build de produção
- `npm run start` - Inicia o servidor em modo de produção
- `npm run lint` - Executa a verificação do ESLint
- `npm run api` - Inicia um servidor de mock (json-server) para auxiliar no desenvolvimento desacoplado

## 🔌 Principais Módulos de API

A API centralizada no backend oferece endpoints para:

- **Autenticação:** Login, Registro, Refresh Token e Reset de Senha.
- **Usuários:** Gestão de perfis, permissões e estatísticas.
- **Conteúdo:** CRUDs para Notícias (com categorias), Eventos (com tipos) e Projetos (com diretorias).
- **Loja:** Gestão de Produtos, Pedidos, Avaliações e **Checkout via Mercado Pago**.

## 🎨 Clean Code e Padrões

- **Tipagem Forte:** TypeScript em todo o frontend para garantir consistência.
- **Tailwind CSS:** Estilização baseada em utilitários para rapidez e consistência visual.
- **Acessibilidade:** Componentes desenvolvidos com foco em semântica e usabilidade.
- **Micro-animações:** Uso de Framer Motion para uma experiência fluida.

---

Feito com ❤️ pela equipe do **DACC**.
