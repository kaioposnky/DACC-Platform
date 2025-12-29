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
        | `email` | `string` | Sim         | E-mail do usuário |
        | `senha` | `string` | Sim         | Senha do usuário  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/auth/login \
    -H "Content-Type: application/json" \
    -d '{
      "email": "usuario@exemplo.com",
      "senha": "minhasenha123"
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
              "nome": "João Silva",
              "email": "usuario@exemplo.com"
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
        | `nome`            | `string`  | Não         | Nome do usuário              |
        | `sobrenome`       | `string`  | Não         | Sobrenome do usuário         |
        | `email`           | `string`  | Não         | E-mail do usuário            |
        | `ra`              | `string`  | Não         | Registro Acadêmico           |
        | `curso`           | `string`  | Não         | Curso do usuário             |
        | `telefone`        | `string`  | Não         | Telefone do usuário          |
        | `senha`           | `string`  | Não         | Senha do usuário             |
        | `imagemUrl`       | `string`  | Não         | URL da imagem de perfil      |
        | `inscritoNoticia` | `boolean` | Não         | Inscrição em newsletter      |
        | `cargo`           | `string`  | Não         | Cargo (aluno/diretor/admin)  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/auth/register \
    -H "Content-Type: application/json" \
    -d '{
      "nome": "João",
      "sobrenome": "Silva",
      "email": "joao.silva@exemplo.com",
      "ra": "123456789",
      "curso": "Ciência da Computação",
      "senha": "minhasenha123",
      "cargo": "aluno"
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
            "nome": "João Silva",
            "email": "joao.silva@exemplo.com"
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
    * **Body (`application/x-www-form-urlencoded`)**

        | Campo          | Tipo     | Obrigatório | Descrição     |
        |----------------|----------|-------------|---------------|
        | `refreshToken` | `string` | Sim         | Refresh token |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/auth/refresh \
    -H "Content-Type: application/x-www-form-urlencoded" \
    -d 'refreshToken=refresh_token_string'
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

* **Descrição:** Realiza logout do usuário (não implementado)
* **Autorização:** Requer permissão `users.logout`

* **Respostas:**
    * **`501 Not Implemented` - Não Implementado**
        ```json
        {
          "success": false,
          "code": "NOT_IMPLEMENTED",
          "message": "Funcionalidade não implementada"
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
              "nome": "João Silva",
              "email": "joao.silva@exemplo.com",
              "ra": "123456789",
              "curso": "Ciência da Computação",
              "cargo": "aluno"
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
            "nome": "João Silva",
            "email": "joao.silva@exemplo.com",
            "ra": "123456789",
            "curso": "Ciência da Computação",
            "cargo": "aluno",
            "telefone": "(11) 99999-9999",
            "inscritoNoticia": true
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
        | `nome`            | `string`    | Não         | Nome do usuário          |
        | `sobrenome`       | `string`    | Não         | Sobrenome do usuário     |
        | `email`           | `string`    | Não         | E-mail do usuário        |
        | `curso`           | `string`    | Não         | Curso do usuário         |
        | `telefone`        | `string`    | Não         | Telefone do usuário      |
        | `imageFile`       | `file`      | Não         | Arquivo de imagem        |
        | `inscritoNoticia` | `boolean`   | Não         | Inscrição em newsletter  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/users/12345678-1234-1234-1234-123456789012 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "nome=João Santos" \
    -F "telefone=(11) 88888-8888"
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
            "nome": "João Santos",
            "telefone": "(11) 88888-8888"
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
              "nome": "Camiseta DACC",
              "descricao": "Camiseta oficial do DACC",
              "categoria": "roupas",
              "subcategoria": "camisetas",
              "preco": 29.90,
              "precoOriginal": 39.90
            }
          ]
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
            "nome": "Camiseta DACC",
            "descricao": "Camiseta oficial do DACC com logo bordado",
            "categoria": "roupas",
            "subcategoria": "camisetas",
            "preco": 29.90,
            "precoOriginal": 39.90,
            "variacoes": [
              {
                "id": "87654321-4321-4321-4321-210987654321",
                "cor": "azul",
                "tamanho": "M",
                "estoque": 10
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
        | `nome`         | `string` | Sim         | Nome do produto (3-50 caracteres)    |
        | `descricao`    | `string` | Sim         | Descrição (10-1000 caracteres)       |
        | `categoria`    | `string` | Sim         | Categoria do produto                 |
        | `subcategoria` | `string` | Sim         | Subcategoria do produto              |
        | `preco`        | `number` | Sim         | Preço do produto (maior que zero)    |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/products \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "nome": "Camiseta DACC",
      "descricao": "Camiseta oficial do DACC com logo bordado",
      "categoria": "roupas",
      "subcategoria": "camisetas",
      "preco": 29.90
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
            "nome": "Camiseta DACC",
            "preco": 29.90
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
              "field": "nome",
              "message": "Nome é obrigatório"
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
                "nome": "Camiseta DACC",
                "preco": 29.90
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

        | Campo           | Tipo     | Obrigatório | Descrição               |
        |-----------------|----------|-------------|-------------------------|
        | `nome`          | `string` | Não         | Nome do produto         |
        | `descricao`     | `string` | Não         | Descrição do produto    |
        | `categoria`     | `string` | Não         | Categoria do produto    |
        | `subcategoria`  | `string` | Não         | Subcategoria do produto |
        | `preco`         | `number` | Não         | Preço atual             |
        | `precoOriginal` | `number` | Não         | Preço original          |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "preco=24.90"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Produto atualizado com sucesso",
          "data": {
            "id": "12345678-1234-1234-1234-123456789012",
            "preco": 24.90
          }
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
        | `cor`           | `string` | Sim         | Nome da cor da variação                                       |
        | `tamanho`       | `string` | Sim         | Tamanho (PP/P/M/G/GG/XG/Pequeno/Médio/Grande)                 |
        | `estoque`       | `number` | Não         | Quantidade em estoque (0-99999, padrão: 0)                    |
        | `ordemVariacao` | `number` | Não         | Ordem de exibição da variação (0-999, padrão: 0)              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012/variations \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "cor=azul" \
    -F "tamanho=M" \
    -F "estoque=50"
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
            "cor": "azul",
            "tamanho": "M",
            "estoque": 50
          }
        }
        ```

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
        | `cor`           | `string` | Não         | Nome da cor da variação                            |
        | `tamanho`       | `string` | Não         | Tamanho (PP/P/M/G/GG/XG/Pequeno/Médio/Grande)      |
        | `estoque`       | `number` | Não         | Quantidade em estoque (0-99999)                    |
        | `ordemVariacao` | `number` | Não         | Ordem de exibição da variação (0-999)              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012/variations/87654321-4321-4321-4321-210987654321 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "estoque=25"
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
            "estoque": 25
          }
        }
        ```

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
        | `imagem`    | `file`   | Sim         | Arquivo de imagem (máximo 5MB)                |
        | `imagemAlt` | `string` | Não         | Texto alternativo (máximo 255 caracteres)     |
        | `ordem`     | `number` | Não         | Ordem de exibição (padrão: 0)                 |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/products/12345678-1234-1234-1234-123456789012/variations/87654321-4321-4321-4321-210987654321/images \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "imagem=@camiseta_azul.jpg" \
    -F "imagemAlt=Camiseta DACC azul tamanho M"
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
            "url": "/uploads/camiseta_azul_123456.jpg",
            "imagemAlt": "Camiseta DACC azul tamanho M",
            "ordem": 0
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
        | `imagem`    | `file`   | Não         | Novo arquivo de imagem (máximo 5MB)         |
        | `imagemAlt` | `string` | Não         | Texto alternativo (máximo 255 caracteres)   |
        | `ordem`     | `number` | Não         | Ordem de exibição                           |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/products/images/11111111-1111-1111-1111-111111111111 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "imagemAlt=Nova descrição da imagem"
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
            "imagemAlt": "Nova descrição da imagem"
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

### **POST /api/orders**

* **Descrição:** Criação de pedido alinhada com o carrinho.
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**
    ```json
    {
      "items": [
        { "id": "uuid_da_variacao", "productId": "uuid_do_produto", "quantity": 1 }
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
            "orderId": "99999999-9999-9999-9999-999999999999",
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
            "orders": {
              "id": "99999999-9999-9999-9999-999999999999",
              "status": "approved",
              "total": 59.80,
              "dataCriacao": "2025-08-08T10:00:00Z",
              "itens": [
                {
                  "produtoNome": "Camiseta DACC",
                  "cor": "azul",
                  "tamanho": "M",
                  "quantidade": 2,
                  "precoUnitario": 29.90
                }
              ]
            }
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
              "total": 59.80,
              "dataCriacao": "2025-08-08T10:00:00Z"
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
*   **Response Data:** `{ "id": "uuid", "codigo": "string", "tipoDesconto": "Percentage", "valor": 10.0 }`

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
        | `nota`       | `number` | Sim         | Nota de 1 a 5 estrelas        |
        | `comentario` | `string` | Não         | Comentário sobre o produto    |
        | `productId`  | `uuid`   | Sim         | ID do produto sendo avaliado  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/ratings \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "nota": 5,
      "comentario": "Produto excelente, recomendo!",
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
            "nota": 5,
            "comentario": "Produto excelente, recomendo!",
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
              "nota": 5,
              "comentario": "Produto excelente!",
              "nomeUsuario": "João Silva",
              "dataCriacao": "2025-08-08T10:00:00Z"
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
        | `usuarioId` | `uuid` | ID único do usuário |

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
              "nota": 5,
              "comentario": "Produto excelente!",
              "produtoNome": "Camiseta DACC",
              "dataCriacao": "2025-08-08T10:00:00Z"
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

### **GET /api/news**

* **Descrição:** Lista todas as notícias publicadas
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/news
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
              "id": "uuid",
              "titulo": "string",
              "autor": { "id": "uuid", "nome": "string", "sobrenome": "string" },
              "tags": [ { "id": "uuid", "nome": "string" } ]
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
            "titulo": "Nova parceria do DACC",
            "descricao": "DACC firma parceria com empresa de tecnologia",
            "conteudo": "Conteúdo completo da notícia...",
            "categoria": "parceria",
            "imagemUrl": "/uploads/noticia_123.jpg",
            "dataPublicacao": "2025-08-08T10:00:00Z",
            "dataAtualizacao": "2025-08-08T11:00:00Z",
            "autorId": "87654321-4321-4321-4321-210987654321",
            "autorNome": "João Silva"
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
    * **Body (`multipart/form-data`)**

        | Campo             | Tipo       | Obrigatório | Descrição                        |
        |-------------------|------------|-------------|----------------------------------|
        | `titulo`          | `string`   | Sim         | Título da notícia                |
        | `descricao`       | `string`   | Sim         | Descrição resumida               |
        | `conteudo`        | `string`   | Não         | Conteúdo completo da notícia     |
        | `categoria`       | `string`   | Não         | Categoria da notícia             |
        | `imageFile`       | `file`     | Não         | Arquivo de imagem (máximo 5MB)   |
        | `dataPublicacao`  | `datetime` | Não         | Data de publicação               |
        | `dataAtualizacao` | `datetime` | Não         | Data de atualização              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/news \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "titulo=Nova parceria do DACC" \
    -F "descricao=DACC firma parceria com empresa de tecnologia" \
    -F "conteudo=Conteúdo completo da notícia..." \
    -F "categoria=parceria" \
    -F "imageFile=@noticia.jpg"
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

    * **Body (`multipart/form-data`)**

        | Campo             | Tipo       | Obrigatório | Descrição                        |
        |-------------------|------------|-------------|----------------------------------|
        | `titulo`          | `string`   | Não         | Novo título da notícia           |
        | `descricao`       | `string`   | Não         | Nova descrição                   |
        | `conteudo`        | `string`   | Não         | Novo conteúdo                    |
        | `categoria`       | `string`   | Não         | Nova categoria                   |
        | `imageFile`       | `file`     | Não         | Nova imagem (máximo 5MB)         |
        | `dataPublicacao`  | `datetime` | Não         | Nova data de publicação          |
        | `dataAtualizacao` | `datetime` | Não         | Nova data de atualização         |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/news/22222222-2222-2222-2222-222222222222 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "titulo=Parceria DACC - Atualizada"
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
            "titulo": "Parceria DACC - Atualizada"
          }
        }
        ```

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

## Events

### **GET /api/events**

* **Descrição:** Lista todos os eventos disponíveis
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/events
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
              "titulo": "Workshop de React",
              "descricao": "Workshop sobre desenvolvimento com React",
              "data": "2025-08-15T14:00:00Z",
              "tipoEvento": "workshop",
              "textoAcao": "Inscrever-se",
              "linkAcao": "https://forms.google.com/workshop-react"
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
            "titulo": "Workshop de React",
            "descricao": "Workshop completo sobre desenvolvimento com React, incluindo hooks e context API",
            "data": "2025-08-15T14:00:00Z",
            "tipoEvento": "workshop",
            "textoAcao": "Inscrever-se",
            "linkAcao": "https://forms.google.com/workshop-react",
            "organizadorId": "87654321-4321-4321-4321-210987654321",
            "organizadorNome": "João Silva"
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

        | Campo        | Tipo       | Obrigatório | Descrição                                       |
        |--------------|------------|-------------|-------------------------------------------------|
        | `titulo`     | `string`   | Não         | Título do evento                                |
        | `descricao`  | `string`   | Não         | Descrição detalhada do evento                   |
        | `data`       | `datetime` | Não         | Data e hora do evento                           |
        | `tipoEvento` | `string`   | Não         | Tipo do evento (workshop/seminário/hackathon)   |
        | `textoAcao`  | `string`   | Não         | Texto do botão de ação                          |
        | `linkAcao`   | `string`   | Não         | Link para inscrição ou mais informações         |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/events \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "titulo": "Workshop de React",
      "descricao": "Workshop sobre desenvolvimento com React",
      "data": "2025-08-15T14:00:00Z",
      "tipoEvento": "workshop",
      "textoAcao": "Inscrever-se",
      "linkAcao": "https://forms.google.com/workshop-react"
    }'
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Evento criado com sucesso",
          "data": {
            "id": "33333333-3333-3333-3333-333333333333",
            "titulo": "Workshop de React",
            "data": "2025-08-15T14:00:00Z"
          }
        }
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

### **GET /api/projects**

* **Descrição:** Lista todos os projetos acadêmicos
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/projects
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
              "id": "44444444-4444-4444-4444-444444444444",
              "titulo": "Sistema de Gestão Acadêmica",
              "descricao": "Sistema para gerenciar atividades acadêmicas",
              "status": "em progresso",
              "diretoria": "Tecnologia",
              "tags": ["web", "backend", "frontend"]
            }
          ]
        }
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
            "titulo": "Sistema de Gestão Acadêmica",
            "descricao": "Sistema completo para gerenciar atividades acadêmicas do DACC",
            "status": "em progresso",
            "diretoria": "Tecnologia",
            "tags": ["web", "backend", "frontend"],
            "imagemUrl": "/uploads/projeto_123.jpg",
            "dataCriacao": "2025-08-01T10:00:00Z",
            "dataAtualizacao": "2025-08-08T10:00:00Z"
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
    * **Body (`multipart/form-data`)**

        | Campo       | Tipo     | Obrigatório | Descrição                                      |
        |-------------|----------|-------------|------------------------------------------------|
        | `titulo`    | `string` | Não         | Título do projeto                              |
        | `descricao` | `string` | Não         | Descrição detalhada do projeto                 |
        | `status`    | `string` | Não         | Status (planejado/em progresso/concluído)      |
        | `diretoria` | `string` | Não         | Diretoria responsável pelo projeto             |
        | `tags`      | `array`  | Não         | Tags relacionadas ao projeto                   |
        | `imageFile` | `file`   | Não         | Arquivo de imagem do projeto (máximo 5MB)      |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/projects \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "titulo=Sistema de Gestão Acadêmica" \
    -F "descricao=Sistema para gerenciar atividades acadêmicas" \
    -F "status=planejado" \
    -F "diretoria=Tecnologia" \
    -F "tags=web" \
    -F "tags=backend" \
    -F "imageFile=@projeto.jpg"
    ```

* **Respostas:**
    * **`201 Created` - Sucesso**
        ```json
        {
          "success": true,
          "code": "CREATED",
          "message": "Projeto criado com sucesso",
          "data": {
            "id": "44444444-4444-4444-4444-444444444444",
            "titulo": "Sistema de Gestão Acadêmica",
            "status": "planejado"
          }
        }
        ```

### **PATCH /api/projects/{id}**

* **Descrição:** Atualiza um projeto existente
* **Autorização:** Requer permissão `projetos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do projeto  |

    * **Body (`multipart/form-data`)**

        | Campo       | Tipo     | Obrigatório | Descrição                                      |
        |-------------|----------|-------------|------------------------------------------------|
        | `titulo`    | `string` | Não         | Novo título do projeto                         |
        | `descricao` | `string` | Não         | Nova descrição                                 |
        | `status`    | `string` | Não         | Novo status                                    |
        | `diretoria` | `string` | Não         | Nova diretoria responsável                     |
        | `tags`      | `array`  | Não         | Novas tags                                     |
        | `imageFile` | `file`   | Não         | Nova imagem (máximo 5MB)                       |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/projects/44444444-4444-4444-4444-444444444444 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "status=em progresso"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Projeto atualizado com sucesso",
          "data": {
            "id": "44444444-4444-4444-4444-444444444444",
            "status": "em progresso"
          }
        }
        ```

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

## Faculty

### **GET /api/faculty**

* **Descrição:** Lista todos os diretores e diretorias
* **Autorização:** Requer permissão `faculty.view`

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/faculty \
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
              "id": "55555555-5555-5555-5555-555555555555",
              "nome": "João Silva",
              "descricao": "Diretor de Tecnologia",
              "email": "joao.silva@dacc.com",
              "githubLink": "https://github.com/joaosilva",
              "linkedinLink": "https://linkedin.com/in/joaosilva",
              "imagemUrl": "/uploads/diretor_123.jpg"
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
        | `id` | `uuid` | ID único do diretor  |

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
            "nome": "João Silva",
            "descricao": "Diretor de Tecnologia responsável por projetos de desenvolvimento",
            "email": "joao.silva@dacc.com",
            "githubLink": "https://github.com/joaosilva",
            "linkedinLink": "https://linkedin.com/in/joaosilva",
            "imagemUrl": "/uploads/diretor_123.jpg",
            "usuarioId": "87654321-4321-4321-4321-210987654321",
            "diretoriaId": "66666666-6666-6666-6666-666666666666"
          }
        }
        ```
    * **`404 Not Found` - Diretor Não Encontrado**
        ```json
        {
          "success": false,
          "code": "RESOURCE_NOT_FOUND",
          "message": "Recurso não encontrado"
        }
        ```

### **POST /api/faculty**

* **Descrição:** Cria um novo diretor
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Body (`multipart/form-data`)**

        | Campo          | Tipo     | Obrigatório | Descrição                           |
        |----------------|----------|-------------|-------------------------------------|
        | `nome`         | `string` | Não         | Nome do diretor                     |
        | `descricao`    | `string` | Não         | Descrição do cargo/responsabilidade |
        | `email`        | `string` | Não         | E-mail do diretor                   |
        | `githubLink`   | `string` | Não         | Link do perfil GitHub               |
        | `linkedinLink` | `string` | Não         | Link do perfil LinkedIn             |
        | `imageFile`    | `file`   | Não         | Foto do diretor (máximo 5MB)        |
        | `usuarioId`    | `uuid`   | Não         | ID do usuário associado             |
        | `diretoriaId`  | `uuid`   | Não         | ID da diretoria                     |

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

### **PATCH /api/faculty/{id}**

* **Descrição:** Atualiza informações de um diretor
* **Autorização:** Requer autenticação JWT

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição            |
        |------|--------|----------------------|
        | `id` | `uuid` | ID único do diretor  |

    * **Body (`multipart/form-data`)**

        | Campo          | Tipo     | Obrigatório | Descrição                           |
        |----------------|----------|-------------|-------------------------------------|
        | `nome`         | `string` | Não         | Novo nome do diretor                |
        | `descricao`    | `string` | Não         | Nova descrição                      |
        | `email`        | `string` | Não         | Novo e-mail                         |
        | `githubLink`   | `string` | Não         | Novo link do GitHub                 |
        | `linkedinLink` | `string` | Não         | Novo link do LinkedIn               |
        | `imageFile`    | `file`   | Não         | Nova foto (máximo 5MB)              |
        | `usuarioId`    | `uuid`   | Não         | Novo ID do usuário associado        |
        | `diretoriaId`  | `uuid`   | Não         | Novo ID da diretoria                |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/faculty/55555555-5555-5555-5555-555555555555 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "descricao=Diretor de Tecnologia e Inovação"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Diretor atualizado com sucesso",
          "data": {
            "id": "55555555-5555-5555-5555-555555555555",
            "descricao": "Diretor de Tecnologia e Inovação"
          }
        }
        ```

### **DELETE /api/faculty/{id}**

* **Descrição:** Remove um diretor do sistema
* **Autorização:** Requer autenticação JWT

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
          "message": "Diretor removido com sucesso"
        }
        ```#
# Anúncios

### **GET /api/announcements**

* **Descrição:** Lista todos os anúncios ativos
* **Autorização:** Requer autenticação JWT

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET http://localhost:3001/v1/api/announcements \
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
              "titulo": "Manutenção do Sistema",
              "conteudo": "Sistema ficará em manutenção no domingo",
              "tipoAnuncio": "importante",
              "ativo": true,
              "imagemUrl": "/uploads/anuncio_123.jpg",
              "imagemAlt": "Ícone de manutenção"
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
            "titulo": "Manutenção do Sistema",
            "conteudo": "O sistema ficará em manutenção no domingo das 8h às 12h",
            "tipoAnuncio": "importante",
            "ativo": true,
            "imagemUrl": "/uploads/anuncio_123.jpg",
            "imagemAlt": "Ícone de manutenção",
            "autorId": "87654321-4321-4321-4321-210987654321",
            "dataCriacao": "2025-08-08T10:00:00Z"
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
    * **Body (`multipart/form-data`)**

        | Campo         | Tipo      | Obrigatório | Descrição                                     |
        |---------------|-----------|-------------|-----------------------------------------------|
        | `titulo`      | `string`  | Não         | Título do anúncio                             |
        | `conteudo`    | `string`  | Não         | Conteúdo do anúncio                           |
        | `tipoAnuncio` | `string`  | Não         | Tipo (evento/notícia/importante)              |
        | `ativo`       | `boolean` | Não         | Se o anúncio está ativo                       |
        | `imageFile`   | `file`    | Não         | Arquivo de imagem (máximo 5MB)                |
        | `imagemAlt`   | `string`  | Não         | Texto alternativo da imagem                   |
        | `autorId`     | `uuid`    | Não         | ID do autor (preenchido automaticamente)      |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/announcements \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "titulo=Manutenção do Sistema" \
    -F "conteudo=Sistema ficará em manutenção no domingo" \
    -F "tipoAnuncio=importante" \
    -F "ativo=true" \
    -F "imageFile=@manutencao.jpg" \
    -F "imagemAlt=Ícone de manutenção"
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
            "titulo": "Manutenção do Sistema",
            "tipoAnuncio": "importante"
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

    * **Body (`multipart/form-data`)**

        | Campo         | Tipo      | Obrigatório | Descrição                        |
        |---------------|-----------|-------------|----------------------------------|
        | `titulo`      | `string`  | Não         | Novo título                      |
        | `conteudo`    | `string`  | Não         | Novo conteúdo                    |
        | `tipoAnuncio` | `string`  | Não         | Novo tipo                        |
        | `ativo`       | `boolean` | Não         | Novo status de ativação          |
        | `imageFile`   | `file`    | Não         | Nova imagem (máximo 5MB)         |
        | `imagemAlt`   | `string`  | Não         | Novo texto alternativo           |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH http://localhost:3001/v1/api/announcements/77777777-7777-7777-7777-777777777777 \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "ativo=false"
    ```

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

* **Descrição:** Faz upload de uma imagem para o servidor
* **Autorização:** Requer role `administrador`

* **Parâmetros da Requisição:**
    * **Body (`multipart/form-data`)**

        | Campo  | Tipo   | Obrigatório | Descrição                      |
        |--------|--------|-------------|--------------------------------|
        | `file` | `file` | Sim         | Arquivo de imagem (máximo 5MB) |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST http://localhost:3001/v1/api/filestorage/uploadImage \
    -H "Authorization: Bearer <seu_jwt_token>" \
    -F "file=@imagem.jpg"
    ```

* **Respostas:**
    * **`200 OK` - Sucesso**
        ```json
        {
          "success": true,
          "code": "OK",
          "message": "Upload realizado com sucesso",
          "data": {
            "url": "/uploads/imagem_123456.jpg"
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