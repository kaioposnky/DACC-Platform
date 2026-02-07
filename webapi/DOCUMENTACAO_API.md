# Documentação da API - DACC

## Visão Geral

A DaccApi é uma API REST completa construída em .NET 7.0 para gerenciar uma plataforma digital acadêmica do Diretório Acadêmico de Ciência da Computação (DACC). A API oferece funcionalidades integradas de gestão acadêmica, e-commerce e gestão de conteúdo.

## Base URL

```
http://localhost:3001/v1/api
```

## Autenticação

A API utiliza **JSON Web Tokens (JWT)** para autenticação. O token deve ser enviado no header `Authorization` com o prefixo `Bearer`:

```
Authorization: Bearer <seu_jwt_token>
```

## Estrutura de Resposta Padronizada

Todas as respostas da API seguem o padrão `ApiResponse`:

### Formato de Sucesso
```json5
{
  "success": true,
  "code": "OK",
  "message": "Requisição bem-sucedida",
  "data": { /* dados retornados */ }
}
```

### Formato de Erro
```json5
{
  "success": false,
  "code": "ERROR_CODE",
  "message": "Descrição do erro",
  "details": [ /* detalhes adicionais se aplicável */ ]
}
```

## Códigos de Status HTTP

- **200 OK**: Requisição bem-sucedida
- **201 Created**: Recurso criado com sucesso
- **204 No Content**: Requisição bem-sucedida, sem conteúdo para retornar
- **400 Bad Request**: Dados inválidos na requisição
- **401 Unauthorized**: Token inválido ou expirado
- **403 Forbidden**: Permissões insuficientes
- **404 Not Found**: Recurso não encontrado
- **409 Conflict**: Recurso já existe
- **413 Payload Too Large**: Arquivo maior que 5MB
- **429 Too Many Requests**: Limite de requisições excedido
- **500 Internal Server Error**: Erro interno do servidor

---

# Endpoints da API
## Autenticação

### **POST /api/auth/login**

* **Descrição:** Realiza login do usuário e retorna token JWT
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**

        | Campo   | Tipo     | Obrigatório | Descrição         |
        |---------|----------|-------------|-------------------|
        | `email` | `string` | Sim         | User email        |
        | `password` | `string` | Sim         | User password  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/auth/login \
    -H "Content-Type: application/json" \
    -d '{
      "email": "user@example.com",
      "password": "mypassword123"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Login realizado com sucesso",
          "data": {
            "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
            "refreshToken": "refresh_token_string",
            "expiresIn": 3600,
            "user": {
              "id": "12345678-1234-1234-1234-123456789012",
              "name": "João Silva",
              "email": "usuario@exemplo.com",
              "role": "aluno"
            }
          }
        }
        ```
    * **`401 Unauthorized` - Credenciais Inválidas**
        ```json
        {
          "success": false,
          "code": "INVALID_CREDENTIALS",
          "message": "Credenciais inválidas"
        }
        ```

### **POST /api/auth/register**

* **Descrição:** Registra um novo usuário no sistema
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**

        | Campo             | Tipo      | Obrigatório | Descrição                    |
        |-------------------|-----------|-------------|------------------------------|
        | `name`            | `string`  | No          | User first name              |
        | `lastName`        | `string`  | No          | User last name               |
        | `email`           | `string`  | No          | User email                   |
        | `ra`              | `string`  | No          | Academic Register (RA)       |
        | `course`          | `string`  | No          | User course                  |
        | `phone`           | `string`  | No          | User phone                   |
        | `password`        | `string`  | No          | User password                |
        | `avatar`          | `string`  | No          | Profile image URL            |
        | `isSubscribedToNews` | `boolean` | No      | Newsletter subscription      |
        | `role`            | `string`  | No          | Role (aluno/diretor/admin)   |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/auth/register \
    -H "Content-Type: application/json" \
    -d '{
      "name": "John",
      "lastName": "Doe",
      "email": "john.doe@example.com",
      "ra": "123456789",
      "course": "Computer Science",
      "password": "mypassword123",
      "role": "student"
    }'
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Usuário registrado com sucesso",
          "data": {
            "id": "12345678-1234-1234-1234-123456789012",
            "name": "João Silva",
            "email": "joao.silva@exemplo.com",
            "role": "aluno"
          }
        }
        ```
    * **`400 Bad Request` - Erro de Validação**
        ```json
        {
          "success": false,
          "code": "VALIDATION_ERROR",
          "message": "Erro de validação dos dados",
          "details": [
            {
              "field": "email",
              "message": "E-mail já está em uso"
            }
          ]
        }
        ```

### **POST /api/auth/refresh**

* **Descrição:** Renova o token JWT usando refresh token
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**

        | Campo          | Tipo     | Obrigatório | Descrição     |
        |----------------|----------|-------------|---------------|
        | `refreshToken` | `string` | Sim         | Refresh token |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/auth/refresh \
    -H "Content-Type: application/json" \
    -d '"refresh_token_string"'
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Token renovado com sucesso",
          "data": {
            "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
            "refreshToken": "new_refresh_token_string",
            "expiresIn": 3600
          }
        }
        ```
    * **`401 Unauthorized` - Token Inválido**
        ```json
        {
          "success": false,
          "code": "AUTH_TOKEN_INVALID",
          "message": "Token JWT inválido"
        }
        ```

### **POST /api/auth/logout**

* **Descrição:** Realiza logout do usuário autenticado
* **Autorização:** Requer permissão `users.logout`

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Logout realizado com sucesso"
        }
        ```

### **POST /api/auth/forgot-password**
*   **Descrição:** Solicita recuperação de senha por e-mail.
*   **Body:** `{ "email": "string" }`

### **GET /api/auth/validate-reset-token**
*   **Descrição:** Valida se um token de reset ainda é válido.
*   **Query:** `token={token}`

### **POST /api/auth/reset-password**
*   **Descrição:** Redefine a senha usando o token recebido.
*   **Body:** `{ "token": "string", "newPassword": "string" }`

### **POST /api/auth/change-password** (Autenticado)
*   **Descrição:** Altera a senha do usuário logado.
*   **Body:** `{ "currentPassword": "string", "newPassword": "string" }`
## Usuários

### **GET /api/users**

* **Descrição:** Lista todos os usuários do sistema
* **Autorização:** Requer permissão `users.viewall`

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/users \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "12345678-1234-1234-1234-123456789012",
              "name": "João Silva",
              "email": "joao.silva@exemplo.com",
              "ra": "123456789",
              "course": "Ciência da Computação",
              "role": "aluno"
            }
          ]
        }
        ```
    * **`403 Forbidden` - Permissões Insuficientes**
        ```json
        {
          "success": false,
          "code": "AUTH_INSUFFICIENT_PERMISSIONS",
          "message": "Permissões insuficientes"
        }
        ```

### **GET /api/users/{id}**

* **Descrição:** Obtém informações de um usuário específico
* **Autorização:** Requer permissão `users.view`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do usuário |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/users/12345678-1234-1234-1234-123456789012 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "12345678-1234-1234-1234-123456789012",
            "name": "João Silva",
            "email": "joao.silva@exemplo.com",
            "ra": "123456789",
            "course": "Ciência da Computação",
            "role": "aluno",
            "phone": "(11) 99999-9999",
            "isSubscribedToNews": true
          }
        }
        ```
    * **`404 Not Found` - Usuário Não Encontrado**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```

### **PATCH /api/users/{id}**

* **Descrição:** Atualiza informações de um usuário
* **Autorização:** Requer permissão `users.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do usuário |

    * **Body (`multipart/form-data`)**

        | Campo             | Tipo        | Obrigatório | Descrição                |
        |-------------------|-------------|-------------|--------------------------|
        | `name`            | `string`    | No          | User name                |
        | `lastName`        | `string`    | No          | User last name           |
        | `email`           | `string`    | No          | User email               |
        | `course`          | `string`    | No          | User course              |
        | `phone`           | `string`    | No          | User phone               |
        | `imageFile`       | `file`      | No          | Image file               |
        | `isSubscribedToNews` | `boolean`   | No          | Newsletter subscription  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/users/12345678-1234-1234-1234-123456789012 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "name=John Santos" \
    -F "phone=(11) 88888-8888"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Usuário atualizado com sucesso",
          "data": {
            "id": "12345678-1234-1234-1234-123456789012",
            "name": "João Santos",
            "phone": "(11) 88888-8888"
          }
        }
        ```
    * **`400 Bad Request` - Erro de Validação**
        ```json
        {
          "success": false,
          "code": "VALIDATION_ERROR",
          "message": "Erro de validação dos dados",
          "details": [
            {
              "field": "email",
              "message": "E-mail inválido"
            }
          ]
        }
        ```

### **DELETE /api/users/{id}**

* **Descrição:** Remove um usuário do sistema
* **Autorização:** Requer permissão `users.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do usuário |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/users/12345678-1234-1234-1234-123456789012 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Usuário removido com sucesso"
        }
        ```
    * **`404 Not Found` - Usuário Não Encontrado**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```
      
### **GET /api/users/{id}/stats**
*   **Descrição:** Obtém estatísticas de perfil do usuário.
*   **Response Data:**
    ```json
    {
      "orders": 10,
      "reviews": 5,
      "averageRating": 4.5,
      "registryDate": "08/08/2025"
    }
    ```
## Cursos

### **GET /api/cursos**

* **Descrição:** Lista todos os cursos disponíveis no sistema
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/cursos
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "cursos": [
              {
                "id": "uuid-123",
                "nome": "Ciência da Computação"
              },
              {
                "id": "uuid-456",
                "nome": "Engenharia de Software"
              }
            ]
          }
        }
        ```

### **GET /api/cursos/{id}**

* **Descrição:** Obtém informações de um curso específico
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**
        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do curso   |

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "curso": {
              "id": "uuid-123",
              "nome": "Ciência da Computação"
            }
          }
        }
        ```

### **POST /api/cursos**

* **Descrição:** Cria um novo curso no sistema
* **Autorização:** Requer permissão `cursos.create`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        | Campo  | Tipo     | Obrigatório | Descrição                    |
        |--------|----------|-------------|------------------------------|
        | `nome` | `string` | Sim         | Nome do curso (max 200 ch) |

* **Respostas:**
    * **`201 Created` - Sucesso**
    * **`400 Bad Request` - Já existe curso com este nome**

### **PATCH /api/cursos/{id}**

* **Descrição:** Atualiza o nome de um curso existente
* **Autorização:** Requer permissão `cursos.update`

* **Parâmetros da Requisição:**
    * **Path**
        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do curso   |
    * **Body (`application/json`)**
        | Campo  | Tipo     | Obrigatório | Descrição                    |
        |--------|----------|-------------|------------------------------|
        | `nome` | `string` | Sim         | Novo nome do curso          |

### **DELETE /api/cursos/{id}**

* **Descrição:** Remove um curso do sistema
* **Autorização:** Requer permissão `cursos.delete`

---

## Roles (Cargos)

### **GET /api/roles**

* **Descrição:** Lista todos os cargos (roles) disponíveis no sistema
* **Autorização:** Público

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "roles": [
              { "id": "uuid", "nome": "aluno" },
              { "id": "uuid", "nome": "diretor" },
              { "id": "uuid", "nome": "administrador" }
            ]
          }
        }
        ```

---

## Products

### **GET /api/products**

* **Descrição:** Lista todos os produtos disponíveis
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/products
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "12345678-1234-1234-1234-123456789012",
              "name": "Camiseta DACC",
              "description": "Camiseta oficial do DACC",
              "category": "roupas",
              "price": 29.90,
              "originalPrice": 39.90,
              "inStock": true
            }
          ]
        }
        ```

### **GET /api/products/subcategorias**

* **Descrição:** Retorna a lista de todas as subcategorias de produtos cadastradas
* **Autorização:** Público

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "data": [ "Camisetas", "Moletons", "Canecas", "Acessórios" ]
        }
        ```

### **GET /api/products/{id}**

* **Descrição:** Obtém informações detalhadas de um produto específico
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do produto |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "12345678-1234-1234-1234-123456789012",
            "name": "Camiseta DACC",
            "description": "Camiseta oficial do DACC com logo bordado",
            "category": "roupas",
            "price": 29.90,
            "originalPrice": 39.90,
            "variations": [
              {
                "id": "87654321-4321-4321-4321-210987654321",
                "color": "azul",
                "size": "M",
                "stock": 10,
                "inStock": true
              }
            ]
          }
        }
        ```
    * **`404 Not Found` - Produto Não Encontrado**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```

### **POST /api/products**

* **Descrição:** Cria um novo produto
* **Autorização:** Requer permissão `produtos.create`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**

        | Campo          | Tipo     | Obrigatório | Descrição                            |
        |----------------|----------|-------------|--------------------------------------|
        | `name`         | `string` | Yes         | Product name (3-50 chars)            |
        | `description`  | `string` | Yes         | Description (10-1000 chars)          |
        | `category`     | `string` | Yes         | Product category                     |
        | `subcategory`  | `string` | Yes         | Product subcategory                  |
        | `price`        | `number` | Yes         | Product price (greater than zero)    |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/products \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "name": "DACC T-Shirt",
      "description": "Official DACC T-Shirt with embroidered logo",
      "category": "clothing",
      "subcategory": "t-shirts",
      "price": 29.90
    }'
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Produto criado com sucesso",
          "data": {
            "id": "12345678-1234-1234-1234-123456789012",
            "name": "DACC T-Shirt",
            "price": 29.90
          }
        }
        ```
    * **`400 Bad Request` - Erro de Validação**
        ```json
        {
          "success": false,
          "code": "VALIDATION_ERROR",
          "message": "Erro de validação dos dados",
          "details": [
            {
              "field": "name",
              "message": "Name is required"
            }
          ]
        }
        ```

### **GET /api/products/search**

* **Descrição:** Busca avançada com filtros.
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Query**

        | Nome           | Tipo     | Padrão    | Descrição                                     |
        |----------------|----------|-----------|-----------------------------------------------|
        | `Page`         | `number` | `1`       | Número da página (maior que 0)                |
        | `Limit`        | `number` | `16`      | Itens por página (1-100)                      |
        | `SearchQuery`  | `string` | -         | Termo de busca (máximo 200 caracteres)        |
        | `Category`     | `string` | -         | Filtro por categoria                          |
        | `MinPrice`     | `number` | -         | Preço mínimo (maior ou igual a 0)             |
        | `MaxPrice`     | `number` | -         | Preço máximo (maior ou igual a 0)             |
        | `OrderBy`      | `string` | `newest`  | Ordenação (price-low/price-high/newest/name)  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "http://localhost:3001/v1/api/products/search?SearchQuery=camiseta&Category=roupas&Page=1&Limit=10"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "produtos": [
              {
                "id": "12345678-1234-1234-1234-123456789012",
                "name": "Camiseta DACC",
                "price": 29.90
              }
            ],
            "totalItens": 1,
            "paginaAtual": 1,
            "totalPaginas": 1
          }
        }
        ```

### **PATCH /api/products/{id}**

* **Descrição:** Atualiza informações de um produto
* **Autorização:** Requer permissão `produtos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do produto |

    * **Body (`multipart/form-data`)**

### **PATCH /api/products/{id}**

* **Descrição:** Atualiza as informações básicas de um produto
* **Autorização:** Requer permissão `produtos.update`

* **Parâmetros da Requisição:**
    * **Path**
        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do produto |

    * **Body (`application/json`)**
        | Campo          | Tipo     | Obrigatório | Descrição                            |
        |----------------|----------|-------------|--------------------------------------|
        | `name`         | `string` | Não         | Novo nome do produto                 |
        | `description`  | `string` | Não         | Nova descrição                       |
        | `price`        | `number` | Não         | Novo preço                           |
        | `category`     | `string` | Não         | Nova categoria                       |
        | `subcategory`  | `string` | Não         | Nova subcategoria                    |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{ "price": 24.90 }'
    ```

### **PATCH /api/products/{id}/batch-update**

* **Descrição:** Realiza uma atualização em massa de todas as propriedades do produto, incluindo especificações, informações de frete e variações (com imagens).
* **Autorização:** Requer permissão `produtos.update`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        | Campo | Tipo | Descrição |
        |-------|------|-----------|
        | `name` | `string` | Nome do produto |
        | `variations` | `array` | Lista de variações com `id`, `color`, `size`, `stock` e `images` (com `url` e `order`) |
        | `specifications` | `array` | Lista de `{ name, value }` |
        | `shippingInfo` | `object` | `{ freeShipping, estimatedDays, returnPolicy }` |

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "message": "Produto atualizado com sucesso"
        }
        ```

### **DELETE /api/products/{id}**

* **Descrição:** Remove um produto do sistema
* **Autorização:** Requer permissão `produtos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do produto |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Produto removido com sucesso"
        }
        ```

### **POST /api/products/{id}/variations**

* **Descrição:** Cria uma nova variação para um produto
* **Autorização:** Requer permissão `produtos.create`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do produto |

    * **Body (`multipart/form-data`)**

        | Campo           | Tipo     | Obrigatório | Descrição                                                     |
        |-----------------|----------|-------------|---------------------------------------------------------------|
        | `color`         | `string` | Yes         | Variation color                               |
        | `size`          | `string` | Yes         | Size (XS/S/M/L/XL/XXL/Small/Medium/Large)     |
        | `stock`         | `number` | No          | Stock quantity (0-99999, default: 0)          |
        | `order`         | `number` | No          | Variation display order (0-999, default: 0)   |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012/variations \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "color=blue" \
    -F "size=M" \
    -F "stock=50"
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Variação criada com sucesso",
          "data": {
            "id": "87654321-4321-4321-4321-210987654321",
            "color": "blue",
            "size": "M",
            "stock": 50
          }
        }
        ```

### **POST /api/products/{id}/variations/json**

* **Descrição:** Cria variação via JSON
* **Autorização:** Requer permissão `produtos.create`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        * `color`, `size`, `stock`, `order`

* **Respostas:**
    * **`201 Created` - Sucesso**

### **GET /api/products/{id}/variations**

* **Descrição:** Lista todas as variações de um produto
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do produto |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012/variations
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "87654321-4321-4321-4321-210987654321",
              "cor": "azul",
              "tamanho": "M",
              "estoque": 50,
              "ordemVariacao": 0
            }
          ]
        }
        ```

### **PATCH /api/products/{id}/variations/{variationId}**

* **Descrição:** Atualiza uma variação específica de produto
* **Autorização:** Requer permissão `produtos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome          | Tipo   | Descrição              |
        |---------------|--------|------------------------|
        | `id`          | `uuid` | ID único do produto    |
        | `variationId` | `uuid` | ID único da variação   |

    * **Body (`multipart/form-data`)**

        | Campo           | Tipo     | Obrigatório | Descrição                                          |
        |-----------------|----------|-------------|----------------------------------------------------|
        | `color`         | `string` | No          | Variation color                               |
        | `size`          | `string` | No          | Size (XS/S/M/L/XL/XXL/Small/Medium/Large)     |
        | `stock`         | `number` | No          | Stock quantity (0-99999)                      |
        | `order`         | `number` | No          | Variation display order (0-999)               |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012/variations/87654321-4321-4321-4321-210987654321 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "stock=25"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Variação atualizada com sucesso",
          "data": {
            "id": "87654321-4321-4321-4321-210987654321",
            "stock": 25
          }
        }
        ```

### **PATCH /api/products/{id}/variations/{variationId}/json**

* **Descrição:** Atualiza uma variação via JSON
* **Autorização:** Requer permissão `produtos.update`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        * `color`, `size`, `stock`, `order`

* **Respostas:**
    * **`200 OK` - Sucesso**

### **DELETE /api/products/{id}/variations/{variationId}**

* **Descrição:** Remove uma variação específica de produto
* **Autorização:** Requer permissão `produtos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome          | Tipo   | Descrição              |
        |---------------|--------|------------------------|
        | `id`          | `uuid` | ID único do produto    |
        | `variationId` | `uuid` | ID único da variação   |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012/variations/87654321-4321-4321-4321-210987654321 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Variação removida com sucesso"
        }
        ```

### **POST /api/products/{productId}/variations/{variationId}/images**

* **Descrição:** Adiciona uma imagem a uma variação de produto
* **Autorização:** Requer permissão `produtos.create`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome          | Tipo   | Descrição              |
        |---------------|--------|------------------------|
        | `productId`   | `uuid` | ID único do produto    |
        | `variationId` | `uuid` | ID único da variação   |

    * **Body (`multipart/form-data`)**

        | Campo       | Tipo     | Obrigatório | Descrição                                     |
        |-------------|----------|-------------|-----------------------------------------------|
        | `image`     | `file`   | Yes         | Image file (max 5MB)                          |
        | `imageAlt`  | `string` | No          | Alternative text (max 255 chars)              |
        | `order`     | `number` | No          | Display order (default: 0)                    |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012/variations/87654321-4321-4321-4321-210987654321/images \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "image=@tshirt_blue.jpg" \
    -F "imageAlt=Blue DACC T-Shirt Size M"
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Imagem adicionada com sucesso",
          "data": {
            "id": "11111111-1111-1111-1111-111111111111",
            "url": "/uploads/tshirt_blue_123456.jpg",
            "imageAlt": "Blue DACC T-Shirt Size M",
            "order": 0
          }
        }
        ```
    * **`413 Payload Too Large` - Arquivo Muito Grande**
        ```json
        {
          "success": false,
          "code": "CONTENT_TOO_LARGE",
          "message": "O arquivo enviado não pode ter mais de 5MB de tamanho"
        }
        ```

### **GET /api/products/images/{imageId}**

* **Descrição:** Obtém informações de uma imagem específica
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo   | Descrição            |
        |-----------|--------|----------------------|
        | `imageId` | `uuid` | ID único da imagem   |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/products/images/11111111-1111-1111-1111-111111111111
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "11111111-1111-1111-1111-111111111111",
            "url": "/uploads/camiseta_azul_123456.jpg",
            "imagemAlt": "Camiseta DACC azul tamanho M",
            "ordem": 0
          }
        }
        ```

### **PATCH /api/products/images/{imageId}**

* **Descrição:** Atualiza informações de uma imagem
* **Autorização:** Requer permissão `produtos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo   | Descrição            |
        |-----------|--------|----------------------|
        | `imageId` | `uuid` | ID único da imagem   |

    * **Body (`multipart/form-data`)**

        | Campo       | Tipo     | Obrigatório | Descrição                                   |
        |-------------|----------|-------------|---------------------------------------------|
        | `image`     | `file`   | No          | New image file (max 5MB)                      |
        | `imageAlt`  | `string` | No          | Alternative text (max 255 chars)              |
        | `order`     | `number` | No          | Display order                                 |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/products/images/11111111-1111-1111-1111-111111111111 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "imageAlt=New image description"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Imagem atualizada com sucesso",
          "data": {
            "id": "11111111-1111-1111-1111-111111111111",
            "imageAlt": "New image description"
          }
        }
        ```

### **DELETE /api/products/images/{imageId}**

* **Descrição:** Remove uma imagem de produto
* **Autorização:** Requer permissão `produtos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo   | Descrição            |
        |-----------|--------|----------------------|
        | `imageId` | `uuid` | ID único da imagem   |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/products/images/11111111-1111-1111-1111-111111111111 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Imagem removida com sucesso"
        }
        ```

## Orders

---

## Cupons (Descontos)

### **GET /api/coupons**
* **Descrição:** Lista todos os cupons cadastrados.
* **Autorização:** Requer permissão `cupons.view`

### **GET /api/coupons/{id}**
* **Descrição:** Obtém detalhes de um cupom específico pelo ID.
* **Autorização:** Requer permissão `cupons.view`

### **POST /api/coupons**
* **Descrição:** Cria um novo cupom de desconto.
* **Autorização:** Requer permissão `cupons.create`
* **Parâmetros da Requisição (Body):**
    | Campo | Tipo | Obrigatório | Descrição |
    |-------|------|-------------|-----------|
    | `code` | `string` | Sim | Código único do cupom |
    | `discountValue` | `decimal` | Sim | Valor do desconto |
    | `type` | `int` | Sim | `0` (Fixo), `1` (Porcentagem) |
    | `expirationDate` | `datetime` | Não | Data de expiração |
    | `usageLimit` | `int` | Não | Limite máximo de utilizadores |

### **PATCH /api/coupons/{id}**
* **Descrição:** Atualiza as informações de um cupom existente.
* **Autorização:** Requer permissão `cupons.update`

### **DELETE /api/coupons/{id}**
* **Descrição:** Remove um cupom do sistema.
* **Autorização:** Requer permissão `cupons.delete`

### **GET /api/orders/validate-coupon/{code}**
* **Descrição:** Valida se um cupom é válido e retorna seus detalhes para aplicação no pedido.
* **Autorização:** Autenticado

---

### **POST /api/orders**

* **Descrição:** Criação de pedido alinhada com o carrinho.
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
    ```json
    {
      "items": [
        { "id": "variation_uuid", "productId": "product_uuid", "quantity": 1 }
      ],
      "couponCode": "PROMO10",
      "deliveryMethod": "CampusDelivery"
    }
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Pedido criado com sucesso",
          "data": {
            "id": "99999999-9999-9999-9999-999999999999",
            "status": "created",
            "total": 59.80,
            "paymentUrl": "https://mercadopago.com/checkout/v1/redirect?pref_id=123456789"
          }
        }
        ```
    * **`400 Bad Request` - Produto Fora de Estoque**
        ```json
        {
          "success": false,
          "code": "PRODUCT_OUT_OF_STOCK",
          "message": "Produto fora de estoque"
        }
        ```

### **GET /api/orders/{id}**

* **Descrição:** Obtém informações de um pedido específico
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição          |
        |------|--------|--------------------|
        | `id` | `uuid` | ID único do pedido |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/orders/99999999-9999-9999-9999-999999999999 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "99999999-9999-9999-9999-999999999999",
            "status": "approved",
            "totalAmount": 59.80,
            "orderDate": "2025-08-08T10:00:00Z",
            "orderItems": [
              {
                "productName": "DACC T-Shirt",
                "color": "blue",
                "size": "M",
                "quantity": 2,
                "unitPrice": 29.90
              }
            ]
          }
        }
        ```
    * **`404 Not Found` - Pedido Não Encontrado**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```

### **GET /api/orders/user/{userId}**

* **Descrição:** Lista todos os pedidos de um usuário específico
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Path**

        | Nome     | Tipo   | Descrição           |
        |----------|--------|---------------------|
        | `userId` | `uuid` | ID único do usuário |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/orders/user/12345678-1234-1234-1234-123456789012 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "99999999-9999-9999-9999-999999999999",
              "status": "approved",
              "totalAmount": 59.80,
              "orderDate": "2025-08-08T10:00:00Z"
            }
          ]
        }
        ```

### **PUT /api/orders/{id}/status**

* **Descrição:** Atualiza o status de um pedido
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição          |
        |------|--------|--------------------|
        | `id` | `uuid` | ID único do pedido |

    * **Body (`application/json`)**

        | Campo    | Tipo     | Obrigatório | Descrição                                                           |
        |----------|----------|-------------|---------------------------------------------------------------------|
        | `status` | `string` | Sim         | Novo status (created/pending/approved/rejected/delivered/cancelled) |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PUT http://localhost:3001/v1/api/orders/99999999-9999-9999-9999-999999999999/status \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '"delivered"'
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Status atualizado com sucesso",
          "data": {
            "orderId": "99999999-9999-9999-9999-999999999999",
            "status": "delivered"
          }
        }
        ```

### **GET /api/orders/search** (Autenticado)
*   **Descrição:** Busca histórico de pedidos com filtros.
*   **Query Params:** `Status`, `StartDate`, `EndDate`, `SearchQuery`, `Page`, `Limit`.

### **GET /api/orders/coupons/{code}**
*   **Descrição:** Valida um cupom de desconto.
*   **Response Data:** `{ "id": "uuid", "code": "string", "discountType": "Percentage", "value": 10.0 }`

### **POST /api/orders/webhook**

* **Descrição:** Webhook para processamento de pagamentos do MercadoPago
* **Autorização:** Público (validação por assinatura)

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome           | Tipo     | Obrigatório | Descrição                    |
        |----------------|----------|-------------|------------------------------|
        | `x-signature`  | `string` | Sim         | Assinatura do webhook        |
        | `x-request-id` | `string` | Sim         | ID da requisição             |

    * **Body (`application/x-www-form-urlencoded`)**

        | Campo   | Tipo     | Obrigatório | Descrição                |
        |---------|----------|-------------|--------------------------|
        | `type`  | `string` | Não         | Tipo do evento           |
        | `topic` | `string` | Não         | Tópico do evento         |
        | `data`  | `object` | Não         | Dados do evento          |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/orders/webhook \
    -H "x-signature: ts=1234567890,v1=signature_hash" \
    -H "x-request-id: request-id-123" \
    -d 'type=payment&data[id]=123456789'
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Pagamento realizado com sucesso"
        }
        ```
    * **`400 Bad Request` - Webhook Inválido**
        ```json
        {
          "success": false,
          "code": "INVALID_WEBHOOK",
          "message": "Webhook inválido"
        }
        ```

## Pagamentos

### **GET /api/payments/success**

* **Descrição:** Página de sucesso do pagamento (callback do MercadoPago)
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Query**

        | Nome                 | Tipo     | Obrigatório | Descrição                    |
        |----------------------|----------|-------------|------------------------------|
        | `external_reference` | `string` | Sim         | Referência externa do pedido |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "http://localhost:3001/v1/api/payments/success?external_reference=order_123456"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Pagamento realizado com sucesso",
          "data": "order_123456"
        }
        ```

### **GET /api/payments/failure**

* **Descrição:** Página de falha do pagamento (callback do MercadoPago)
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Query**

        | Nome                 | Tipo     | Obrigatório | Descrição                    |
        |----------------------|----------|-------------|------------------------------|
        | `external_reference` | `string` | Sim         | Referência externa do pedido |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "http://localhost:3001/v1/api/payments/failure?external_reference=order_123456"
    ```

* **Respostas:**
    * **`200 OK` - Falha no Pagamento**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Pagamento falhou. Tente novamente.",
          "data": "order_123456"
        }
        ```

### **GET /api/payments/pending**

* **Descrição:** Página de pagamento pendente (callback do MercadoPago)
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Query**

        | Nome                 | Tipo     | Obrigatório | Descrição                    |
        |----------------------|----------|-------------|------------------------------|
        | `external_reference` | `string` | Sim         | Referência externa do pedido |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "http://localhost:3001/v1/api/payments/pending?external_reference=order_123456"
    ```

* **Respostas:**
    * **`200 OK` - Pagamento Pendente**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Pagamento pendente. Aguarde a confirmação.",
          "data": "order_123456"
        }
        ```

## Ratings

### **GET /api/ratings**

* **Descrição:** Lista todas as avaliações do sistema
* **Autorização:** Requer permissão `reviews.view`

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/ratings \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "11111111-1111-1111-1111-111111111111",
              "nota": 5,
              "comentario": "Produto excelente!",
              "productId": "12345678-1234-1234-1234-123456789012",
              "usuarioId": "87654321-4321-4321-4321-210987654321",
              "dataCriacao": "2025-08-08T10:00:00Z"
            }
          ]
        }
        ```

### **GET /api/ratings/{id}**

* **Descrição:** Obtém informações de uma avaliação específica
* **Autorização:** Requer permissão `reviews.view`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição              |
        |------|--------|------------------------|
        | `id` | `uuid` | ID único da avaliação  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/ratings/11111111-1111-1111-1111-111111111111 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "11111111-1111-1111-1111-111111111111",
            "nota": 5,
            "comentario": "Produto excelente!",
            "productId": "12345678-1234-1234-1234-123456789012",
            "usuarioId": "87654321-4321-4321-4321-210987654321",
            "dataCriacao": "2025-08-08T10:00:00Z"
          }
        }
        ```

### **POST /api/ratings**

* **Descrição:** Cria uma nova avaliação de produto
* **Autorização:** Requer permissão `reviews.create`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**

        | Campo        | Tipo     | Obrigatório | Descrição                     |
        |--------------|----------|-------------|-------------------------------|
        | `rating`     | `number` | Yes         | Rating from 1 to 5 stars      |
        | `comment`    | `string` | No          | Comment about the product     |
        | `productId`  | `uuid`   | Yes         | ID of the product being reviewed |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/ratings \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "rating": 5,
      "comment": "Excellent product, highly recommended!",
      "productId": "12345678-1234-1234-1234-123456789012"
    }'
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Avaliação criada com sucesso",
          "data": {
            "id": "11111111-1111-1111-1111-111111111111",
            "rating": 5,
            "comment": "Excellent product, highly recommended!",
            "productId": "12345678-1234-1234-1234-123456789012"
          }
        }
        ```
    * **`400 Bad Request` - Erro de Validação**
        ```json
        {
          "success": false,
          "code": "VALIDATION_ERROR",
          "message": "Erro de validação dos dados",
          "details": [
            {
              "field": "nota",
              "message": "A nota deve ser de 1 a 5"
            }
          ]
        }
        ```

### **GET /api/ratings/products/{productId}**

* **Descrição:** Lista todas as avaliações de um produto específico
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome        | Tipo   | Descrição           |
        |-------------|--------|---------------------|
        | `productId` | `uuid` | ID único do produto |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/ratings/products/12345678-1234-1234-1234-123456789012
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "11111111-1111-1111-1111-111111111111",
              "rating": 5,
              "comment": "Excellent product!",
              "userName": "John Silva",
              "createdAt": "2025-08-08T10:00:00Z"
            }
          ]
        }
        ```

### **GET /api/ratings/users/{usuarioId}**

* **Descrição:** Lista todas as avaliações feitas por um usuário específico
* **Autorização:** Requer permissão `reviews.view`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome        | Tipo   | Descrição           |
        |-------------|--------|---------------------|
        | `userId`    | `uuid` | ID único do usuário |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/ratings/users/87654321-4321-4321-4321-210987654321 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "11111111-1111-1111-1111-111111111111",
              "rating": 5,
              "comment": "Excellent product!",
              "productName": "DACC T-Shirt",
              "createdAt": "2025-08-08T10:00:00Z"
            }
          ]
        }
        ```

### **PATCH /api/ratings/{id}**

* **Descrição:** Atualiza uma avaliação existente
* **Autorização:** Requer permissão `reviews.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição              |
        |------|--------|------------------------|
        | `id` | `uuid` | ID único da avaliação  |

    * **Body (`application/json`)**

        | Campo         | Tipo     | Obrigatório | Descrição                    |
        |---------------|----------|-------------|------------------------------|
        | `nota`        | `number` | Sim         | Nova nota de 1 a 5 estrelas  |
        | `comentario`  | `string` | Não         | Novo comentário              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/ratings/11111111-1111-1111-1111-111111111111 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "nota": 4,
      "comentario": "Produto bom, mas pode melhorar"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Avaliação atualizada com sucesso",
          "data": {
            "id": "11111111-1111-1111-1111-111111111111",
            "nota": 4,
            "comentario": "Produto bom, mas pode melhorar"
          }
        }
        ```

### **DELETE /api/ratings/{id}**

* **Descrição:** Remove uma avaliação do sistema
* **Autorização:** Requer permissão `reviews.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição              |
        |------|--------|------------------------|
        | `id` | `uuid` | ID único da avaliação  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/ratings/11111111-1111-1111-1111-111111111111 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Avaliação removida com sucesso"
        }
        ```

## Notícias

### **Categorias de Notícia**

Endpoints para gerenciar as categorias das notícias.

#### **GET /api/news/categories**
* **Descrição:** Lista todas as categorias de notícia cadastradas.
* **Autorização:** Público (Autenticado)

#### **POST /api/news/categories**
* **Descrição:** Cria uma nova categoria de notícia.
* **Autorização:** Requer permissão `noticias.categorias.create`
* **Body:** `{ "nome": "string" }`

#### **PATCH /api/news/categories/{id}**
* **Descrição:** Atualiza o nome de uma categoria.
* **Autorização:** Requer permissão `noticias.categorias.update`

#### **DELETE /api/news/categories/{id}**
* **Descrição:** Remove uma categoria.
* **Autorização:** Requer permissão `noticias.categorias.delete`

---

### **GET /api/news**

* **Descrição:** Lista todas as notícias publicadas com suporte a filtros e paginação.
* **Autorização:** Público

* **Parâmetros de Consulta (Query String):**
    | Nome | Tipo | Descrição |
    |------|------|-----------|
    | `searchQuery` | `string` | Busca no título ou descrição |
    | `category` | `string` | Filtra por categoria |
    | `authorId` | `uuid` | Filtra por autor |
    | `startDate`| `date` | Data mínima (yyyy-mm-dd) |
    | `endDate`  | `date` | Data máxima (yyyy-mm-dd) |
    | `page`     | `int`  | Número da página (padrão: 1) |
    | `limit`    | `int`  | Itens por página (padrão: 16) |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "http://localhost:3001/v1/api/news?category=acadêmico&limit=5"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "22222222-2222-2222-2222-222222222222",
              "title": "Parceria DACC",
              "description": "Nova parceria firmada",
              "author": "Diretoria DACC",
              "image": "/uploads/noticia_1.jpg",
              "date": "2025-08-08T10:00:00Z",
              "category": "parceria"
            }
          ]
        }
        ```

### **GET /api/news/{id}**

* **Descrição:** Obtém informações detalhadas de uma notícia específica
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único da notícia  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/news/22222222-2222-2222-2222-222222222222
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "22222222-2222-2222-2222-222222222222",
            "title": "New DACC Partnership",
            "description": "DACC signs partnership with tech company",
            "content": "Full news content...",
            "category": "partnership",
            "imageUrl": "/uploads/news_123.jpg",
            "publishDate": "2025-08-08T10:00:00Z",
            "updatedAt": "2025-08-08T11:00:00Z",
            "authorId": "87654321-4321-4321-4321-210987654321",
            "authorName": "John Silva"
          }
        }
        ```
    * **`404 Not Found` - Notícia Não Encontrada**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```

### **POST /api/news**

* **Descrição:** Cria uma nova notícia
* **Autorização:** Requer permissão `noticias.create`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**

        | Campo             | Tipo       | Obrigatório | Descrição                        |
        |-------------------|------------|-------------|----------------------------------|
        | `title`           | `string`   | Yes         | News title                       |
        | `description`     | `string`   | Yes         | Short description                |
        | `content`         | `string`   | No          | Full content                     |
        | `category`        | `string`   | No          | News category                    |
        | `publishDate`     | `datetime` | No          | Publication date                 |
        | `updatedAt`       | `datetime` | No          | Update date                      |

> [!NOTE]
> O upload da imagem de capa deve ser feito separadamente através do endpoint `PATCH /api/news/{id}/image`.

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/news \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "titulo": "Nova parceria do DACC",
      "descricao": "DACC firma parceria com empresa de tecnologia",
      "conteudo": "Conteúdo completo da notícia...",
      "categoria": "parceria"
    }'
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Notícia criada com sucesso",
          "data": {
            "id": "22222222-2222-2222-2222-222222222222",
            "titulo": "Nova parceria do DACC",
            "categoria": "parceria"
          }
        }
        ```
    * **`400 Bad Request` - Erro de Validação**
        ```json
        {
          "success": false,
          "code": "VALIDATION_ERROR",
          "message": "Erro de validação dos dados",
          "details": [
            {
              "field": "titulo",
              "message": "Título é obrigatório"
            }
          ]
        }
        ```

### **PATCH /api/news/{id}**

* **Descrição:** Atualiza uma notícia existente
* **Autorização:** Requer permissão `noticias.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único da notícia  |

    * **Body (`application/json`)**

        | Campo             | Tipo       | Obrigatório | Descrição                        |
        |-------------------|------------|-------------|----------------------------------|
        | `title`           | `string`   | Não         | Novo título da notícia           |
        | `description`     | `string`   | Não         | Nova descrição                   |
        | `content`         | `string`   | Não         | Novo conteúdo                    |
        | `category`        | `string`   | Não         | Nova categoria                   |
        | `imageUrl`        | `string`   | Não         | Nova imagem (Base64 data URL)    |
        | `publishDate`     | `datetime` | Não         | Nova data de publicação          |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/news/22222222-2222-2222-2222-222222222222 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "title": "Parceria DACC - Atualizada",
      "category": "parceria"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Notícia atualizada com sucesso",
          "data": {
            "id": "22222222-2222-2222-2222-222222222222",
            "title": "Parceria DACC - Atualizada"
          }
        }
        ```

### **PATCH /api/news/{id}/json**

* **Descrição:** Atualiza uma notícia existente via JSON
* **Autorização:** Requer permissão `noticias.update`

* **Parâmetros da Requisição:**
    * **Path**
        * `id`: UUID da notícia
    * **Body (`application/json`)**
        * Same fields as PATCH (titulo, descricao, conteudo, categoria, dataPublicacao, dataAtualizacao)

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/news/{id}/json \
    -H "Authorization: Bearer <token>" \
    -H "Content-Type: application/json" \
    -d '{ "titulo": "Novo Título" }'
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**

### **DELETE /api/news/{id}**

* **Descrição:** Remove uma notícia do sistema
* **Autorização:** Requer permissão `noticias.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único da notícia  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/news/22222222-2222-2222-2222-222222222222 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Notícia removida com sucesso"
        }
        ```

## Eventos

### **Tipos de Evento**

Endpoints para gerenciar os tipos de eventos (ex: Workshop, Palestra, Competição).

#### **GET /api/events/types**
* **Descrição:** Lista todos os tipos de evento cadastrados.
* **Autorização:** Público (Autenticado)

#### **POST /api/events/types**
* **Descrição:** Cria um novo tipo de evento.
* **Autorização:** Requer permissão `eventos.tiposevento.create`

#### **PATCH /api/events/types/{id}**
* **Descrição:** Atualiza um tipo de evento.
* **Autorização:** Requer permissão `eventos.tiposevento.update`

#### **DELETE /api/events/types/{id}**
* **Descrição:** Remove um tipo de evento do sistema.
* **Autorização:** Requer permissão `eventos.tiposevento.delete`

---

### **GET /api/events**

* **Descrição:** Lista todos os eventos disponíveis
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/events
    ```

### **GET /api/events/search**

* **Descrição:** Busca avançada de eventos com filtros.
* **Autorização:** Público

* **Parâmetros de Consulta:**
    | Nome | Tipo | Descrição |
    |------|------|-----------|
    | `searchQuery` | `string` | Busca no título |
    | `eventType` | `string` | Filtra por tipo |
    | `startDate` | `date` | Data inicial |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "http://localhost:3001/v1/api/events/search?eventType=workshop"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "33333333-3333-3333-3333-333333333333",
              "title": "React Workshop",
              "description": "Workshop on React development",
              "date": "2025-08-15T14:00:00Z",
              "eventType": "workshop",
              "actionText": "Register",
              "actionLink": "https://forms.google.com/workshop-react"
            }
          ]
        }
        ```

### **GET /api/events/{id}**

* **Descrição:** Obtém informações detalhadas de um evento específico
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do evento  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/events/33333333-3333-3333-3333-333333333333
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "33333333-3333-3333-3333-333333333333",
            "title": "React Workshop",
            "description": "Complete workshop on React development, including hooks and context API",
            "date": "2025-08-15T14:00:00Z",
            "eventType": "workshop",
            "actionText": "Register",
            "actionLink": "https://forms.google.com/workshop-react",
            "organizerId": "87654321-4321-4321-4321-210987654321",
            "organizerName": "John Silva"
          }
        }
        ```
    * **`404 Not Found` - Evento Não Encontrado**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```

### **POST /api/events**

* **Descrição:** Cria um novo evento
* **Autorização:** Requer permissão `eventos.create`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        | Campo | Tipo | Obrigatório | Descrição |
        |-------|------|-------------|-----------|
        | `title` | `string` | Sim | Título do evento |
        | `description` | `string` | Sim | Descrição detalhada |
        | `date` | `datetime` | Sim | Data e hora |
        | `eventType` | `string` | Não | Tipo (workshop/seminar/hackathon) |
        | `actionText` | `string` | Não | Texto do botão |
        | `actionLink` | `string` | Não | Link de ação |
        | `imageUrl` | `string` | Não | URL ou Base64 da imagem |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/events \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "title": "Workshop de React",
      "description": "Explorando Hooks e Context API",
      "date": "2025-08-15T14:00:00Z",
      "eventType": "workshop"
    }'
    ```

### **PATCH /api/events/{id}**

* **Descrição:** Atualiza um evento existente
* **Autorização:** Requer permissão `eventos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do evento  |

    * **Body (`application/json`)**

        | Campo        | Tipo       | Obrigatório | Descrição                                      |
        |--------------|------------|-------------|------------------------------------------------|
        | `titulo`     | `string`   | Não         | Novo título do evento                          |
        | `descricao`  | `string`   | Não         | Nova descrição                                 |
        | `data`       | `datetime` | Não         | Nova data e hora                               |
        | `tipoEvento` | `string`   | Não         | Novo tipo do evento                            |
        | `textoAcao`  | `string`   | Não         | Novo texto do botão                            |
        | `linkAcao`   | `string`   | Não         | Novo link de ação                              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/events/33333333-3333-3333-3333-333333333333 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "titulo": "Workshop Avançado de React"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Evento atualizado com sucesso",
          "data": {
            "id": "33333333-3333-3333-3333-333333333333",
            "titulo": "Workshop Avançado de React"
          }
        }
        ```

### **DELETE /api/events/{id}**

* **Descrição:** Remove um evento do sistema
* **Autorização:** Requer permissão `eventos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do evento  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/events/33333333-3333-3333-3333-333333333333 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Evento removido com sucesso"
        }
        ```

### **POST /api/events/{id}/register**

* **Descrição:** Registra o usuário em um evento (não implementado)
* **Autorização:** Requer permissão `eventos.register`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do evento  |

* **Respostas:**
    * **`501 Not Implemented` - Não Implementado**
        ```json
        {
          "success": false,
          "code": "NOT_IMPLEMENTED",
          "message": "Funcionalidade não implementada"
        }
        ```

### **DELETE /api/events/{id}/register**

* **Descrição:** Remove o registro do usuário de um evento (não implementado)
* **Autorização:** Requer permissão `eventos.register`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do evento  |

* **Respostas:**
    * **`501 Not Implemented` - Não Implementado**
        ```json
        {
          "success": false,
          "code": "NOT_IMPLEMENTED",
          "message": "Funcionalidade não implementada"
        }
        ```#
## Projetos

### **Tipos de Progresso**

Endpoints para gerenciar os tipos de progresso de projetos (ex: Em Planejamento, Em Execução, Concluído).

#### **GET /api/projects/progress-types**
* **Descrição:** Lista todos os tipos de progresso cadastrados.
* **Autorização:** Público (Autenticado)

#### **POST /api/projects/progress-types**
* **Descrição:** Cria um novo tipo de progresso.
* **Autorização:** Requer permissão `projetos.tiposprogresso.create`

#### **PATCH /api/projects/progress-types/{id}**
* **Descrição:** Atualiza um tipo de progresso.
* **Autorização:** Requer permissão `projetos.tiposprogresso.update`

#### **DELETE /api/projects/progress-types/{id}**
* **Descrição:** Remove um tipo de progresso.
* **Autorização:** Requer permissão `projetos.tiposprogresso.delete`

---

### **GET /api/projects**

* **Descrição:** Lista todos os projetos acadêmicos
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/projects
    ```

### **GET /api/projects/search**

* **Descrição:** Busca avançada de projetos com filtros.
* **Autorização:** Público

* **Parâmetros de Consulta:**
    | Nome | Tipo | Descrição |
    |------|------|-----------|
    | `searchQuery` | `string` | Busca no título |
    | `status` | `string` | Filtra por status |
    | `directorateId` | `uuid` | Filtra por diretoria |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "http://localhost:2222/v1/api/projects/search?status=in%20progress"
    ```

### **GET /api/projects/{id}**

* **Descrição:** Obtém informações detalhadas de um projeto específico
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do projeto  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/projects/44444444-4444-4444-4444-444444444444
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "44444444-4444-4444-4444-444444444444",
            "title": "Academic Management System",
            "description": "Complete system to manage DACC academic activities",
            "status": "in progress",
            "department": "Technology",
            "tags": ["web", "backend", "frontend"],
            "imageUrl": "/uploads/project_123.jpg",
            "createdAt": "2025-08-01T10:00:00Z",
            "updatedAt": "2025-08-08T10:00:00Z"
          }
        }
        ```
    * **`404 Not Found` - Projeto Não Encontrado**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```

### **POST /api/projects**

* **Descrição:** Cria um novo projeto acadêmico
* **Autorização:** Requer permissão `projetos.create`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        | Campo | Tipo | Obrigatório | Descrição |
        |-------|------|-------------|-----------|
        | `title` | `string` | Sim | Título do projeto |
        | `description` | `string` | Sim | Descrição detalhada |
        | `status` | `string` | Não | Status (planned/in progress/completed) |
        | `directorateId` | `uuid` | Não | ID da diretoria responsável |
        | `tags` | `array` | Não | Tags relacionadas |
        | `imageUrl` | `string` | Não | URL ou Base64 da imagem |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/projects \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "title": "Sistema de Gestão Acadêmica",
      "description": "Sistema para gerenciar atividades acadêmicas",
      "status": "planned",
      "directorateId": "87654321-4321-4321-4321-210987654321"
    }'
    ```

### **PATCH /api/projects/{id}**

* **Descrição:** Atualiza um projeto existente
* **Autorização:** Requer permissão `projetos.update`

* **Parâmetros da Requisição:**
    * **Path**
        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do projeto  |

    * **Body (`application/json`)**
        | Campo | Tipo | Obrigatório | Descrição |
        |-------|------|-------------|-----------|
        | `title` | `string` | Não | Novo título |
        | `description` | `string` | Não | Nova descrição |
        | `status` | `string` | Não | Novo status |
        | `directorateId` | `uuid` | Não | Nova diretoria |
        | `tags` | `array` | Não | Novas tags |
        | `imageUrl` | `string` | Não | URL ou Base64 da imagem |
        | `progress` | `int` | Não | Progresso (0-100) |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/projects/44444444-4444-4444-4444-444444444444 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{ "status": "completed", "progress": 100 }'
    ```

### **POST /api/projects/{id}**

* **Descrição:** Adiciona ou atualiza a imagem de um projeto via JSON (Base64).
* **Autorização:** Requer permissão `projetos.update`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        | Campo | Tipo | Descrição |
        |-------|------|-----------|
        | `imageUrl` | `string` | String Base64 da imagem |
        | `imageAlt` | `string` | Texto alternativo |

### **DELETE /api/projects/{id}**

* **Descrição:** Remove um projeto do sistema
* **Autorização:** Requer permissão `projetos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do projeto  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/projects/44444444-4444-4444-4444-444444444444 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Projeto removido com sucesso"
        }
        ```

## Faculty (Corpo Docente)

### **GET /api/faculty**

* **Descrição:** Lista todos os professores (faculty)
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/faculty
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "55555555-5555-5555-5555-555555555555",
              "name": "João Silva",
              "title": "Prof. Dr.",
              "position": "Diretor de Tecnologia",
              "specialization": "Desenvolvimento de Software",
              "image": "/uploads/professor_123.jpg",
              "social": {
                "linkedin": "https://linkedin.com/in/johnsilva",
                "github": "https://github.com/johnsilva",
                "email": "john.silva@dacc.com"
              }
            }
          ]
        }
        ```
    * **`403 Forbidden` - Permissões Insuficientes**
        ```json
        {
          "success": false,
          "code": "AUTH_INSUFFICIENT_PERMISSIONS",
          "message": "Permissões insuficientes"
        }
        ```

### **GET /api/faculty/{id}**

* **Descrição:** Obtém informações detalhadas de um diretor específico
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do professor |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/faculty/55555555-5555-5555-5555-555555555555 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "55555555-5555-5555-5555-555555555555",
            "name": "John Silva",
            "description": "Technology Director responsible for development projects",
            "email": "john.silva@dacc.com",
            "github": "https://github.com/johnsilva",
            "linkedin": "https://linkedin.com/in/johnsilva",
            "image": "/uploads/professor_123.jpg",
            "userId": "87654321-4321-4321-4321-210987654321"
          }
        }
        ```
    * **`404 Not Found` - Professor Não Encontrado**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```

### **POST /api/faculty**

* **Descrição:** Cria um novo professor
* **Autorização:** Requer permissão `faculty.create`

* **Parâmetros da Requisição:**
    * **Body (`multipart/form-data`)**

        | Campo          | Tipo     | Obrigatório | Descrição                           |
        |----------------|----------|-------------|-------------------------------------|
        | `name`         | `string` | Sim         | Nome do professor                   |
        | `title`        | `string` | Sim         | Título (Ex: Dr, Ms)                 |
        | `position`     | `string` | Sim         | Cargo                               |
        | `specialization` | `string` | Sim         | Especialização                      |
        | `email`        | `string` | Não         | Email de contato                    |
        | `github`       | `string` | Não         | Link do GitHub                      |
        | `linkedin`     | `string` | Não         | Link do LinkedIn                    |
        | `imageUrl`     | `string` | Não         | URL ou Base64 da imagem             |
        | `userId`       | `uuid`   | Não         | ID do usuário vinculado             |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/faculty \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "nome=João Silva" \
    -F "descricao=Diretor de Tecnologia" \
    -F "email=joao.silva@dacc.com" \
    -F "githubLink=https://github.com/joaosilva" \
    -F "imageFile=@diretor.jpg"
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Diretor criado com sucesso",
          "data": {
            "id": "55555555-5555-5555-5555-555555555555",
            "nome": "João Silva",
            "email": "joao.silva@dacc.com"
          }
        }
        ```

### **POST /api/faculty/json**

* **Descrição:** Cria um novo professor via JSON
* **Autorização:** Requer permissão `faculty.create`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        * `name`, `description`, `email`, `githubLink`, `linkedinLink`, `userId`, `boardId`

* **Respostas:**
    * **`201 Created` - Sucesso**

### **PATCH /api/faculty/{id}**

* **Descrição:** Atualiza as informações de um diretor
* **Autorização:** Requer permissão `faculty.update`

* **Parâmetros da Requisição:**
    * **Path**
        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do diretor  |

    * **Body (`application/json`)**
        | Campo | Tipo | Obrigatório | Descrição |
        |-------|------|-------------|-----------|
        | `name` | `string` | Não | Nome do professor |
        | `title` | `string` | Não | Título (Ex: Dr, Ms) |
        | `position` | `string` | Não | Cargo |
        | `specialization` | `string` | Não | Especialização |
        | `imageUrl` | `string` | Não | Image Base64 ou URL |
        | `email` | `string` | Não | E-mail de contato |
        | `linkedin` | `string` | Não | Link do LinkedIn |
        | `github` | `string` | Não | Link do GitHub |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/faculty/55555555-5555-5555-5555-555555555555 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{ "position": "Diretor Adjunto" }'
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Professor atualizado com sucesso"
        }
        ```

### **DELETE /api/faculty/{id}**

* **Descrição:** Remove um professor do sistema
* **Autorização:** Requer permissão `faculty.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do diretor  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/faculty/55555555-5555-5555-5555-555555555555 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Professor removido com sucesso"
        }
        ```#
# Anúncios

### **Tipos de Anúncio**

Endpoints para gerenciar os tipos de anúncios (ex: Venda, Doação, Troca).

#### **GET /api/announcements/types**
* **Descrição:** Lista todos os tipos de anúncio cadastrados.
* **Autorização:** Público

#### **POST /api/announcements/types**
* **Descrição:** Cria um novo tipo de anúncio.
* **Autorização:** Requer permissão `anuncios.tiposanuncio.create`

#### **PATCH /api/announcements/types/{id}**
* **Descrição:** Atualiza um tipo de anúncio.
* **Autorização:** Requer permissão `anuncios.tiposanuncio.update`

#### **DELETE /api/announcements/types/{id}**
* **Descrição:** Remove um tipo de anúncio.
* **Autorização:** Requer permissão `anuncios.tiposanuncio.delete`

---

### **GET /api/announcements**

* **Descrição:** Lista todos os anúncios ativos
* **Autorização:** Requer autenticação JWT

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/announcements \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

### **GET /api/announcements/search**

* **Descrição:** Busca avançada de anúncios com filtros.
* **Autorização:** Requer autenticação JWT

* **Parâmetros de Consulta:**
    | Nome | Tipo | Descrição |
    |------|------|-----------|
    | `searchQuery` | `string` | Busca no título |
    | `type` | `string` | Filtra por tipo (evento/notícia/importante) |
    | `isActive` | `bool` | Filtra por status |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "http://localhost:3001/v1/api/announcements/search?type=importante" \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": [
            {
              "id": "77777777-7777-7777-7777-777777777777",
              "title": "Manutenção do Sistema",
              "content": "Sistema ficará em manutenção no domingo",
              "type": "importante",
              "active": true,
              "imageSrc": "/uploads/anuncio_123.jpg",
              "imageAlt": "Ícone de manutenção"
            }
          ]
        }
        ```

### **GET /api/announcements/{id}**

* **Descrição:** Obtém informações detalhadas de um anúncio específico
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do anúncio  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/announcements/77777777-7777-7777-7777-777777777777 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "id": "77777777-7777-7777-7777-777777777777",
            "title": "Manutenção do Sistema",
            "content": "O sistema ficará em manutenção no domingo das 8h às 12h",
            "type": "importante",
            "active": true,
            "imageSrc": "/uploads/anuncio_123.jpg",
            "imageAlt": "Ícone de manutenção",
            "createdAt": "2025-08-08T10:00:00Z"
          }
        }
        ```
    * **`404 Not Found` - Anúncio Não Encontrado**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```

### **POST /api/announcements**

* **Descrição:** Cria um novo anúncio
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        | Campo | Tipo | Obrigatório | Descrição |
        |-------|------|-------------|-----------|
        | `title` | `string` | Sim | Título do anúncio |
        | `content` | `string` | Sim | Conteúdo do anúncio |
        | `type` | `string` | Não | Tipo (evento/notícia/importante) |
        | `isActive` | `bool` | Não | Status de ativação |
        | `imageUrl` | `string` | Não | URL ou Base64 da imagem |
        | `imageAlt` | `string` | Não | Texto alternativo |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/announcements \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "title": "Manutenção do Sistema",
      "content": "Sistema ficará em manutenção no domingo",
      "type": "importante",
      "active": true
    }'
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Anúncio criado com sucesso",
          "data": {
            "id": "77777777-7777-7777-7777-777777777777",
            "title": "Manutenção do Sistema",
            "type": "importante"
          }
        }
        ```

### **PATCH /api/announcements/{id}**

* **Descrição:** Atualiza um anúncio existente
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Path**
        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do anúncio  |

    * **Body (`application/json`)**
        | Campo | Tipo | Obrigatório | Descrição |
        |-------|------|-------------|-----------|
        | `title` | `string` | Não | Novo título |
        | `content` | `string` | Não | Novo conteúdo |
        | `type` | `string` | Não | Novo tipo |
        | `isActive` | `bool` | Não | Novo status |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/announcements/77777777-7777-7777-7777-777777777777 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{ "active": false }'
    ```

### **POST /api/announcements/{id}**

* **Descrição:** Adiciona ou atualiza a imagem de um anúncio via JSON (Base64).
* **Autorização:** Requer permissão `anuncios.update`

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
        | Campo | Tipo | Descrição |
        |-------|------|-----------|
        | `imageUrl` | `string` | String Base64 da imagem |
        | `imageAlt` | `string` | Texto alternativo |

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Anúncio atualizado com sucesso",
          "data": {
            "id": "77777777-7777-7777-7777-777777777777",
            "ativo": false
          }
        }
        ```

### **DELETE /api/announcements/{id}**

* **Descrição:** Remove um anúncio do sistema
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do anúncio  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE http://localhost:3001/v1/api/announcements/77777777-7777-7777-7777-777777777777 \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`204 No Content` - Sucesso**
        ```json
        {
          "success": true,
          "code": "NO_CONTENT",
          "message": "Anúncio removido com sucesso"
        }
        ```

## Upload de Arquivos

### **POST /api/filestorage/uploadImage**

* **Descrição:** Faz upload de uma imagem via `multipart/form-data`
* **Autorização:** Requer role `administrador`

* **Parâmetros da Requisição:**
    * **Body (`multipart/form-data`)**
        | Campo  | Tipo   | Obrigatório | Descrição                      |
        |--------|--------|-------------|--------------------------------|
        | `file` | `file` | Sim         | Arquivo de imagem (máximo 5MB) |

### **POST /api/filestorage/uploadBase64**

* **Descrição:** Faz upload de uma imagem enviada como string Base64.
* **Autorização:** Requer role `administrador`

* **Parâmetros da Requisição:**
    * **Body (`text/plain`)**
        * String bruta contendo o conteúdo Base64 (com ou sem prefixo `data:image/...`).

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Upload realizado com sucesso",
          "data": {
            "url": "http://servidor/uploads/imagem_123.jpg"
          }
        }
        ```
    * **`400 Bad Request` - Nenhum Arquivo Enviado**
        ```json
        {
          "success": false,
          "code": "BAD_REQUEST",
          "message": "Nenhum arquivo foi enviado"
        }
        ```
    * **`413 Payload Too Large` - Arquivo Muito Grande**
        ```json
        {
          "success": false,
          "code": "CONTENT_TOO_LARGE",
          "message": "O arquivo enviado não pode ter mais de 5MB de tamanho"
        }
        ```

## Estatísticas

### **GET /api/statistics/dashboard**

* **Descrição:** Retorna estatísticas gerais do sistema para o dashboard administrativo.
* **Autorização:** Requer permissão `dashboard.view`

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/statistics/dashboard \
    -H "Authorization: Bearer <seu_jwt_token>"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Requisição bem-sucedida",
          "data": {
            "users": {
              "total": 1500,
              "active": 1450,
              "subscribers": 800,
              "newThisMonth": 45,
              "byRole": {
                "admin": 5,
                "diretor": 10,
                "aluno": 1485
              }
            },
            "orders": {
              "total": 350,
              "totalRevenue": 15490.50,
              "pending": 12,
              "salesLast30Days": 45,
              "byStatus": {
                "pending": 12,
                "approved": 200,
                "delivered": 138
              }
            },
            "products": {
              "totalActive": 120,
              "lowStockCount": 5,
              "byCategory": {
                "Roupas": 80,
                "Acessórios": 40
              }
            },
            "reviews": {
              "total": 85,
              "averageRating": 4.8,
              "ratingDistribution": {
                "5": 70,
                "4": 10,
                "3": 5
              }
            },
            "events": {
              "total": 12,
              "upcoming": 2,
              "byType": {
                "Workshop": 5,
                "Palestra": 7
              }
            },
            "news": {
              "total": 45,
              "byCategory": {
                "Acadêmico": 30,
                "Eventos": 15
              }
            },
            "ads": {
              "totalActive": 8,
              "byType": {
                "Banner": 4,
                "Sidebar": 4
              }
            },
            "faculty": {
              "total": 15,
              "byTitle": {
                "Presidente": 1,
                "Diretor": 14
              }
            },
            "permissions": {
              "totalDefinitions": 56
            }
          }
        }
        ```
    * **`403 Forbidden` - Permissões Insuficientes**
        ```json
        {
          "success": false,
          "code": "AUTH_INSUFFICIENT_PERMISSIONS",
          "message": "Permissões insuficientes"
        }
        ```

---

## Códigos de Erro Específicos

### Erros de Autenticação
- `AUTH_TOKEN_INVALID` (401) - Token JWT inválido
- `AUTH_TOKEN_EXPIRED` (401) - Token JWT expirado
- `AUTH_INSUFFICIENT_PERMISSIONS` (403) - Permissões insuficientes
- `INVALID_CREDENTIALS` (401) - Credenciais inválidas

### Erros de Validação
- `VALIDATION_ERROR` (400) - Erro de validação com detalhes específicos
- `BAD_REQUEST` (400) - Dados inválidos na requisição
- `RESOURCE_NOT_FOUND` (404) - Recurso não encontrado
- `RESOURCE_ALREADY_EXISTS` (409) - Recurso já existe

### Erros Específicos do Domínio
- `ACCOUNT_INACTIVE` (400) - Conta desativada
- `INSUFFICIENT_STOCK` (400) - Estoque insuficiente
- `PRODUCT_OUT_OF_STOCK` (400) - Produto fora de estoque
- `CART_ITEM_NOT_FOUND` (404) - Item não encontrado no carrinho
- `EVENT_FULL` (400) - Evento lotado
- `REGISTRATION_CLOSED` (400) - Inscrições encerradas
- `CONTENT_TOO_LARGE` (413) - Arquivo maior que 5MB
- `PAYMENT_FAILED` (400) - Falha no processamento do pagamento
- `INVALID_WEBHOOK` (400) - Webhook inválido

### Erros Técnicos
- `INTERNAL_SERVER_ERROR` (500) - Erro interno do servidor
- `RATE_LIMIT_EXCEEDED` (429) - Limite de requisições excedido

---

*Documentação gerada automaticamente em 08/08/2025*