# DACC-Platform Website

Frontend da Plataforma Digital Acadêmica do DACC, desenvolvido com **Next.js 16** e **TypeScript**.

## 🚀 Tecnologias

- **Framework:** Next.js 16 (App Router)
- **Linguagem:** TypeScript
- **Estilização:** Tailwind CSS 4
- **Animações:** Framer Motion
- **Notificações:** Sonner

## 🏗️ Arquitetura (Atomic Design)

O projeto segue o padrão **Atomic Design** para organização de componentes:

```
website/src/
├── app/                    # Diretório de rotas do Next.js
├── components/             # Componentes organizados por complexidade
│   ├── atoms/             # Menores unidades (Botões, Inputs, Typography)
│   ├── molecules/         # Combinações de átomos (SearchBar, NewsCard)
│   ├── organisms/         # Seções complexas (Forms, Navigation, Footer)
│   └── templates/         # Layouts de página
├── context/               # Gerenciamento de estado global (Auth, etc.)
├── services/              # Integração com a Web API (.NET)
├── types/                 # Definições de interfaces TypeScript
└── utils/                 # Funções utilitárias e formatadores
```

## 🛠️ Começando

### Pré-requisitos
- Node.js 18+
- npm ou yarn

### Instalação e Execução

1. Navegue até a pasta `website`:
   ```bash
   cd website
   ```
2. Instale as dependências:
   ```bash
   npm install
   ```
3. Execute o servidor de desenvolvimento:
   ```bash
   npm run dev
   ```

## 📝 Scripts Disponíveis

- `npm run dev` - Inicia o servidor Next.js com Turbopack.
- `npm run build` - Gera a build de produção.
- `npm run start` - Inicia o servidor em modo produção.
- `npm run lint` - Executa a verificação do ESLint.
- `npm run api` - Inicia o mock server (`json-server`) na porta 3001.
- `npm run dev:full` - Inicia o Next.js e o Mock Server simultaneamente.
- `npm run deploy` - Script automatizado para pull, build e restart via PM2.

## 🎨 Padrões de Código

- **Tipagem Estrita:** Uso obrigatório de TypeScript para garantir segurança e consistência.
- **Tailwind CSS:** Estilização utilitária para manter a agilidade no desenvolvimento.
- **Componentização:** Foco em reuso e separação de responsabilidades.

---

Feito com ❤️ pela equipe do **DACC**.
