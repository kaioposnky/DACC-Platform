# DaccApi - API do Diretório Acadêmico de Ciência da Computação

[![.NET](https://img.shields.io/badge/.NET-7.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/) 
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=flat-square&logo=postgresql&logoColor=white)](https://www.postgresql.org/) 
[![JWT](https://img.shields.io/badge/JWT-000000?style=flat-square&logo=JSON%20web%20tokens&logoColor=white)](https://jwt.io/) 
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black)](https://swagger.io/) 

Uma API REST completa construída em .NET 7.0 para gerenciar uma **plataforma digital acadêmica** integrada, combinando funcionalidades de gestão acadêmica e e-commerce para o Diretório Acadêmico de Ciência da Computação da FEI.

## 🛠️ Stack Tecnológica

| Tecnologia                | Finalidade                 |
|---------------------------|----------------------------|
| **.NET Core**             | Framework principal da API |
| **Entity Framework Core** | ORM principal              |
| **Dapper**                | Queries na Database        |
| **NHibernate**            | Mapeamentos complexos      |
| **JWT Bearer**            | Autenticação e autorização |
| **Argon2**                | Hash seguro de senhas      |
| **ImageSharp**            | Processamento de imagens   |
| **MercadoPago SDK**       | Gateway de pagamento       |
| **Swagger**               | Documentação da API        |

## 📁 Estrutura do Projeto

```
DaccApi/
├── Controllers/          # Endpoints organizados por domínio
│   ├── Auth/            # Autenticação e autorização
│   ├── Usuario/         # Gestão de usuários
│   ├── Produtos/        # Catálogo e e-commerce
│   ├── Orders/          # Processamento de pedidos
│   ├── Payments/        # Integração de pagamentos
│   └── ...              # Outros domínios
├── Services/            # Lógica de negócio
├── Infrastructure/      # Camada de infraestrutura
│   ├── Authorization/   # Sistema de permissões
│   ├── Repositories/    # Acesso a dados
│   ├── Cryptography/    # Serviços de criptografia
│   └── MercadoPago/     # Integração de pagamento
├── Model/               # DTOs e entidades
│   ├── Objects/         # Entidades de domínio
│   ├── Requests/        # DTOs de entrada
│   └── Responses/       # DTOs de saída
├── Helpers/             # Utilitários e helpers
├── Queries/             # Mapeamentos NHibernate
└── wwwroot/uploads/     # Arquivos de upload
```

## 🏗️ Arquitetura

### Padrões Implementados

- **Repository Pattern**: Separação de acesso a dados
- **Service Layer**: Lógica de negócio centralizada
- **Dependency Injection**: Inversão de controle
- **Response Pattern**: Respostas padronizadas
- **Authorization Attributes**: Controle de acesso granular

### ORMs Utilizados

- **Entity Framework Core**: CRUD principal e migrações
- **Dapper**: Queries otimizadas e performance
- **NHibernate**: Mapeamentos complexos (arquivos .hbm.xml)

## 🤝 Contribuição

Você é muito bem-vindo para dar sugestões, melhorar o código ou adicionar novas funcionalidades para nossa API! \
Para fazer isso:
1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request


## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

**Desenvolvido com ❤️ para o Diretório Acadêmico de Ciência da Computação da FEI**