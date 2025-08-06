# Documentação da API DaccApi

Documentação API BETA

## Visão Geral

A DaccApi é uma API REST completa construída em .NET 7.0 para gerenciar uma plataforma digital acadêmica do Diretório Acadêmico de Ciência da Computação (DACC) da FEI. A API oferece funcionalidades integradas de gestão acadêmica, e-commerce e gestão de conteúdo.

**Base URL:** `https://api.dacc.com`

## Autenticação

Esta API utiliza **JSON Web Tokens (JWT)** para autenticação. Para acessar endpoints protegidos, inclua o token no header Authorization:

```
Authorization: Bearer <seu_jwt_token>
```

## Estrutura de Resposta Padronizada

Todas as respostas da API seguem o formato `ApiResponse`:

### Respostas de Sucesso
```json5
{
  "success": true,
  "response": {
    "message": "Requisição bem-sucedida",
    "data": { /* dados retornados */ }
  }
}
```

### Respostas de Erro
```json5
{
  "success": false,
  "response": {
    "code": "ERROR_CODE",
    "message": "Descrição do erro",
    "details": [ 
      {field: "id", message: "Id é obrigatório!"}
      /* detalhes adicionais se aplicável */ 
    ]
  }
}
```

---

## Endpoints

### **POST /api/auth/login**

* **Descrição:** Autentica um usuário e retorna tokens de acesso
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**

        | Campo   | Tipo     | Obrigatório | Descrição         |
        |---------|----------|-------------|-------------------|
        | `email` | `string` | Sim         | E-mail do usuário |
        | `senha` | `string` | Sim         | Senha do usuário  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/auth/login \
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
          "response": {
            "message": "Login realizado com sucesso",
            "data": {
              "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
              "refreshToken": "refresh_token_string"
            }
          }
        }
        ```
    * **`401 Unauthorized` - Credenciais Inválidas**
        ```json
        {
          "success": false,
          "response": {
            "code": "INVALID_CREDENTIALS",
            "message": "Credenciais inválidas"
          }
        }
        ```

---###
 **POST /api/auth/register**

* **Descrição:** Registra um novo usuário no sistema
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Body (`application/json`)**

        | Campo                  | Tipo      | Obrigatório | Descrição                             |
        |------------------------|-----------|-------------|---------------------------------------|
        | `nome`                 | `string`  | Não         | Nome do usuário                       |
        | `sobrenome`            | `string`  | Não         | Sobrenome do usuário                  |
        | `email`                | `string`  | Não         | E-mail único do usuário               |
        | `ra`                   | `string`  | Não         | Registro Acadêmico                    |
        | `curso`                | `string`  | Não         | Curso do usuário                      |
        | `telefone`             | `string`  | Não         | Telefone de contato                   |
        | `senha`                | `string`  | Não         | Senha do usuário                      |
        | `imagemUrl`            | `string`  | Não         | URL da imagem de perfil               |
        | `newsLetterSubscriber` | `boolean` | Não         | Inscrição na newsletter               |
        | `cargo`                | `string`  | Não         | Cargo (aluno, diretor, administrador) |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/auth/register \
    -H "Content-Type: application/json" \
    -d '{
      "nome": "João",
      "sobrenome": "Silva",
      "email": "joao.silva@exemplo.com",
      "ra": "123456789",
      "curso": "Ciência da Computação",
      "telefone": "(11) 99999-9999",
      "senha": "minhasenha123",
      "cargo": "aluno",
      "newsLetterSubscriber": true
    }'
    ```

* **Respostas:**
    * **`201 Created` - Usuário Criado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Usuário registrado com sucesso",
            "data": {
              "id": "550e8400-e29b-41d4-a716-446655440000",
              "nome": "João",
              "sobrenome": "Silva",
              "email": "joao.silva@exemplo.com",
              "ra": "123456789",
              "cargo": "aluno"
            }
          }
        }
        ```
    * **`400 Bad Request` - Erro de Validação**
        ```json
        {
          "success": false,
          "response": {
            "code": "VALIDATION_ERROR",
            "message": "Erro de validação dos dados",
            "details": [
              {
                "field": "email",
                "message": "E-mail já está em uso"
              }
            ]
          }
        }
        ```

---

### **POST /api/auth/refresh**

* **Descrição:** Renova o token de acesso usando refresh token
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Body (`application/x-www-form-urlencoded`)**

        | Campo          | Tipo     | Obrigatório | Descrição          |
        |----------------|----------|-------------|--------------------|
        | `refreshToken` | `string` | Sim         | Token de renovação |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/auth/refresh \
    -H "Content-Type: application/x-www-form-urlencoded" \
    -d 'refreshToken=your_refresh_token_here'
    ```

* **Respostas:**
    * **`200 OK` - Token Renovado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Token renovado com sucesso",
            "data": {
              "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
              "refreshToken": "new_refresh_token_string"
            }
          }
        }
        ```
    * **`401 Unauthorized` - Token Inválido**
        ```json
        {
          "success": false,
          "response": {
            "code": "AUTH_TOKEN_INVALID",
            "message": "Token JWT inválido"
          }
        }
        ```

---

### **POST /api/auth/logout**

* **Descrição:** Realiza logout do usuário (não implementado)
* **Autorização:** Requer permissão `users.logout`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome            | Tipo     | Descrição    |
        |-----------------|----------|--------------|
        | `Authorization` | `string` | Bearer token |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/auth/logout \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`501 Not Implemented` - Não Implementado**
        ```json
        {
          "success": false,
          "response": {
            "code": "NOT_IMPLEMENTED",
            "message": "Funcionalidade não implementada"
          }
        }
        ```

---

### **GET /api/users**

* **Descrição:** Lista todos os usuários do sistema
* **Autorização:** Requer permissão `users.viewall`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome            | Tipo     | Descrição    |
        |-----------------|----------|--------------|
        | `Authorization` | `string` | Bearer token |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/users \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Lista de Usuários**
        ```json
        {
          "success": true,
          "response": {
            "message": "Usuários obtidos com sucesso",
            "data": [
              {
                "id": "550e8400-e29b-41d4-a716-446655440000",
                "nome": "João",
                "sobrenome": "Silva",
                "ra": "123456789",
                "email": "joao.silva@exemplo.com",
                "telefone": "(11) 99999-9999",
                "cargo": "aluno",
                "dataCriacao": "2024-01-15T10:30:00Z",
                "dataAtualizacao": "2024-01-15T10:30:00Z"
              }
            ]
          }
        }
        ```
    * **`403 Forbidden` - Permissões Insuficientes**
        ```json
        {
          "success": false,
          "response": {
            "code": "AUTH_INSUFFICIENT_PERMISSIONS",
            "message": "Permissões insuficientes"
          }
        }
        ```

---

### **GET /api/users/{id}**

* **Descrição:** Obtém um usuário específico por ID
* **Autorização:** Requer permissão `users.view`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do usuário |

    * **Headers**

        | Nome            | Tipo     | Descrição    |
        |-----------------|----------|--------------|
        | `Authorization` | `string` | Bearer token |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/users/550e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Usuário Encontrado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Usuário obtido com sucesso",
            "data": {
              "id": "550e8400-e29b-41d4-a716-446655440000",
              "nome": "João",
              "sobrenome": "Silva",
              "ra": "123456789",
              "email": "joao.silva@exemplo.com",
              "telefone": "(11) 99999-9999",
              "cargo": "aluno",
              "dataCriacao": "2024-01-15T10:30:00Z",
              "dataAtualizacao": "2024-01-15T10:30:00Z"
            }
          }
        }
        ```
    * **`404 Not Found` - Usuário Não Encontrado**
        ```json
        {
          "success": false,
          "response": {
            "code": "RESOURCE_NOT_FOUND",
            "message": "Usuário não encontrado"
          }
        }
        ```

---

### **PATCH /api/users/{id}**

* **Descrição:** Atualiza dados de um usuário específico
* **Autorização:** Requer permissão `users.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome | Tipo   | Descrição           |
        |------|--------|---------------------|
        | `id` | `uuid` | ID único do usuário |

    * **Headers**

        | Nome            | Tipo     | Descrição    |
        |-----------------|----------|--------------|
        | `Authorization` | `string` | Bearer token |

    * **Body (`application/json`)**

        | Campo                  | Tipo      | Obrigatório | Descrição                 |
        |------------------------|-----------|-------------|---------------------------|
        | `nome`                 | `string`  | Não         | Nome do usuário           |
        | `sobrenome`            | `string`  | Não         | Sobrenome do usuário      |
        | `email`                | `string`  | Não         | E-mail do usuário         |
        | `curso`                | `string`  | Não         | Curso do usuário          |
        | `telefone`             | `string`  | Não         | Telefone de contato       |
        | `imagemUrl`            | `string`  | Não         | URL da imagem de perfil   |
        | `newsLetterSubscriber` | `boolean` | Não         | Inscrição na newsletter   |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/users/550e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "nome": "João Carlos",
      "telefone": "(11) 88888-8888"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Usuário Atualizado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Usuário atualizado com sucesso",
            "data": {
              "id": "550e8400-e29b-41d4-a716-446655440000",
              "nome": "João Carlos",
              "sobrenome": "Silva",
              "telefone": "(11) 88888-8888",
              "dataAtualizacao": "2024-01-16T14:20:00Z"
            }
          }
        }
        ```

---

### **DELETE /api/users/{id}**

* **Descrição:** Remove um usuário do sistema
* **Autorização:** Requer permissão `users.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do usuário  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/users/550e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Usuário Removido**
        ```json
        {
          "success": true,
          "response": {
            "message": "Usuário removido com sucesso"
          }
        }
        ```

---### **G
ET /api/produtos**

* **Descrição:** Lista todos os produtos disponíveis
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/produtos
    ```

* **Respostas:**
    * **`200 OK` - Lista de Produtos**
        ```json
        {
          "success": true,
          "response": {
            "message": "Produtos obtidos com sucesso",
            "data": {
              "products": [
                {
                  "id": "550e8400-e29b-41d4-a716-446655440000",
                  "nome": "Camiseta DACC",
                  "descricao": "Camiseta oficial do DACC",
                  "preco": 35.90,
                  "categoria": "roupas",
                  "subcategoria": "camisetas",
                  "variacoes": [
                    {
                      "id": "660e8400-e29b-41d4-a716-446655440000",
                      "cor": "azul",
                      "tamanho": "M",
                      "estoque": 10
                    }
                  ],
                  "dataCriacao": "2024-01-15T10:30:00Z"
                }
              ]
            }
          }
        }
        ```

---

### **GET /api/produtos/{id}**

* **Descrição:** Obtém detalhes de um produto específico
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do produto  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/produtos/550e8400-e29b-41d4-a716-446655440000
    ```

* **Respostas:**
    * **`200 OK` - Produto Encontrado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Produto obtido com sucesso",
            "data": {
              "product": {
                "id": "550e8400-e29b-41d4-a716-446655440000",
                "nome": "Camiseta DACC",
                "descricao": "Camiseta oficial do DACC em algodão 100%",
                "preco": 35.90,
                "categoria": "roupas",
                "subcategoria": "camisetas",
                "variacoes": [
                  {
                    "id": "660e8400-e29b-41d4-a716-446655440000",
                    "cor": "azul",
                    "tamanho": "M",
                    "estoque": 10,
                    "imagens": [
                      {
                        "url": "https://exemplo.com/imagem1.jpg",
                        "altText": "Camiseta DACC azul frente"
                      }
                    ]
                  }
                ],
                "dataCriacao": "2024-01-15T10:30:00Z"
              }
            }
          }
        }
        ```
    * **`404 Not Found` - Produto Não Encontrado**
        ```json
        {
          "success": false,
          "response": {
            "code": "RESOURCE_NOT_FOUND",
            "message": "Nenhum produto foi encontrado com esse id"
          }
        }
        ```

---

### **GET /api/produtos/search**

* **Descrição:** Busca produtos com filtros e paginação
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Query**

        | Nome             | Tipo      | Padrão    | Descrição                                        |
        |------------------|-----------|-----------|--------------------------------------------------|
        | `page`           | `number`  | `1`       | Número da página                                 |
        | `limit`          | `number`  | `16`      | Itens por página (máx: 100)                      |
        | `searchPattern`  | `string`  | -         | Padrão de busca no nome/descrição                |
        | `categoria`      | `string`  | -         | Filtro por categoria                             |
        | `subcategoria`   | `string`  | -         | Filtro por subcategoria                          |
        | `minPrice`       | `number`  | -         | Preço mínimo                                     |
        | `maxPrice`       | `number`  | -         | Preço máximo                                     |
        | `sortBy`         | `string`  | `newest`  | Ordenação (price-low, price-high, newest, name)  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "https://api.dacc.com/api/produtos/search?searchPattern=camiseta&categoria=roupas&page=1&limit=10"
    ```

* **Respostas:**
    * **`200 OK` - Resultados da Busca**
        ```json
        {
          "success": true,
          "response": {
            "message": "Produtos encontrados com sucesso",
            "data": {
              "products": [
                {
                  "id": "550e8400-e29b-41d4-a716-446655440000",
                  "nome": "Camiseta DACC",
                  "descricao": "Camiseta oficial do DACC",
                  "preco": 35.90,
                  "categoria": "roupas",
                  "subcategoria": "camisetas"
                }
              ]
            }
          }
        }
        ```
    * **`404 Not Found` - Nenhum Produto Encontrado**
        ```json
        {
          "success": false,
          "response": {
            "code": "RESOURCE_NOT_FOUND",
            "message": "Nenhum produto encontrado com os critérios de busca"
          }
        }
        ```

---

### **POST /api/produtos**

* **Descrição:** Cria um novo produto
* **Autorização:** Requer permissão `produtos.create`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo           | Tipo      | Obrigatório  | Descrição                                  |
        |-----------------|-----------|--------------|--------------------------------------------|
        | `nome`          | `string`  | Sim          | Nome do produto (3-50 caracteres)          |
        | `descricao`     | `string`  | Sim          | Descrição do produto (10-1000 caracteres)  |
        | `categoria`     | `string`  | Sim          | Categoria do produto                       |
        | `subcategoria`  | `string`  | Sim          | Subcategoria do produto                    |
        | `preco`         | `number`  | Sim          | Preço do produto (> 0)                     |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/produtos \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "nome": "Caneca DACC",
      "descricao": "Caneca de porcelana com logo do DACC",
      "categoria": "outros",
      "subcategoria": "canecas",
      "preco": 25.90
    }'
    ```

* **Respostas:**
    * **`201 Created` - Produto Criado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Produto criado com sucesso! Use o endpoint de variações para adicionar opções de compra",
            "data": {
              "product": {
                "id": "770e8400-e29b-41d4-a716-446655440000",
                "nome": "Caneca DACC",
                "descricao": "Caneca de porcelana com logo do DACC",
                "preco": 25.90,
                "categoria": "outros",
                "subcategoria": "canecas",
                "dataCriacao": "2024-01-16T15:30:00Z"
              }
            }
          }
        }
        ```
    * **`400 Bad Request` - Erro de Validação**
        ```json
        {
          "success": false,
          "response": {
            "code": "VALIDATION_ERROR",
            "message": "Erro de validação dos dados",
            "details": [
              {
                "field": "nome",
                "message": "Nome é obrigatório"
              }
            ]
          }
        }
        ```

---

### **PATCH /api/produtos/{id}**

* **Descrição:** Atualiza um produto existente
* **Autorização:** Requer permissão `produtos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do produto  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`multipart/form-data`)**

        | Campo            | Tipo      | Obrigatório  | Descrição                |
        |------------------|-----------|--------------|--------------------------|
        | `nome`           | `string`  | Não          | Nome do produto          |
        | `descricao`      | `string`  | Não          | Descrição do produto     |
        | `categoria`      | `string`  | Não          | Categoria do produto     |
        | `subcategoria`   | `string`  | Não          | Subcategoria do produto  |
        | `preco`          | `number`  | Não          | Preço do produto         |
        | `precoOriginal`  | `number`  | Não          | Preço original           |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/produtos/770e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -F 'nome=Caneca DACC Premium' \
    -F 'preco=27.90'
    ```

* **Respostas:**
    * **`200 OK` - Produto Atualizado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Produto atualizado com sucesso"
          }
        }
        ```

---

### **DELETE /api/produtos/{id}**

* **Descrição:** Remove um produto do catálogo
* **Autorização:** Requer permissão `produtos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do produto  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/produtos/770e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Produto Removido**
        ```json
        {
          "success": true,
          "response": {
            "message": "Produto removido com sucesso"
          }
        }
        ```

---

### **POST /api/produtos/{id}/variations**

* **Descrição:** Cria uma nova variação para um produto
* **Autorização:** Requer permissão `produtos.create`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do produto  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`multipart/form-data`)**

        | Campo            | Tipo      | Obrigatório  | Descrição                                              |
        |------------------|-----------|--------------|--------------------------------------------------------|
        | `cor`            | `string`  | Sim          | Cor da variação                                        |
        | `tamanho`        | `string`  | Sim          | Tamanho (PP, P, M, G, GG, XG, Pequeno, Medio, Grande)  |
        | `estoque`        | `number`  | Sim          | Quantidade em estoque (0-99999)                        |
        | `ordemVariacao`  | `number`  | Não          | Ordem da variação (0-999)                              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/produtos/770e8400-e29b-41d4-a716-446655440000/variations \
    -H "Authorization: Bearer <seu_token>" \
    -F 'cor=vermelho' \
    -F 'tamanho=G' \
    -F 'estoque=15'
    ```

* **Respostas:**
    * **`201 Created` - Variação Criada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Variação criada com sucesso! Use o endpoint de imagens para adicionar fotos",
            "data": {
              "variation": {
                "id": "880e8400-e29b-41d4-a716-446655440000",
                "cor": "vermelho",
                "tamanho": "G",
                "estoque": 15,
                "produtoId": "770e8400-e29b-41d4-a716-446655440000"
              }
            }
          }
        }
        ```
    * **`409 Conflict` - Variação Já Existe**
        ```json
        {
          "success": false,
          "response": {
            "code": "RESOURCE_ALREADY_EXISTS",
            "message": "Já existe uma variação com cor 'vermelho' e tamanho 'G' para este produto"
          }
        }
        ```

---

### **GET /api/produtos/{id}/variations**

* **Descrição:** Lista todas as variações de um produto
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do produto  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/produtos/770e8400-e29b-41d4-a716-446655440000/variations
    ```

* **Respostas:**
    * **`200 OK` - Lista de Variações**
        ```json
        {
          "success": true,
          "response": {
            "message": "Encontradas 2 variações para o produto",
            "data": {
              "variations": [
                {
                  "id": "880e8400-e29b-41d4-a716-446655440000",
                  "cor": "vermelho",
                  "tamanho": "G",
                  "estoque": 15,
                  "imagens": [
                    {
                      "url": "https://exemplo.com/variacao1.jpg",
                      "altText": "Camiseta vermelha tamanho G"
                    }
                  ]
                }
              ]
            }
          }
        }
        ```

---

### **PATCH /api/produtos/{id}/variations/{variationId}**

* **Descrição:** Atualiza uma variação de produto
* **Autorização:** Requer permissão `produtos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome           | Tipo      | Descrição             |
        |----------------|-----------|-----------------------|
        | `id`           | `uuid`    | ID único do produto   |
        | `variationId`  | `uuid`    | ID único da variação  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`multipart/form-data`)**

        | Campo            | Tipo      | Obrigatório  | Descrição              |
        |------------------|-----------|--------------|------------------------|
        | `cor`            | `string`  | Não          | Cor da variação        |
        | `tamanho`        | `string`  | Não          | Tamanho da variação    |
        | `estoque`        | `number`  | Não          | Quantidade em estoque  |
        | `ordemVariacao`  | `number`  | Não          | Ordem da variação      |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/produtos/770e8400-e29b-41d4-a716-446655440000/variations/880e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -F 'estoque=20'
    ```

* **Respostas:**
    * **`200 OK` - Variação Atualizada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Variação atualizada com sucesso",
            "data": {
              "variation": {
                "id": "880e8400-e29b-41d4-a716-446655440000",
                "cor": "vermelho",
                "tamanho": "G",
                "estoque": 20
              }
            }
          }
        }
        ```

---

### **DELETE /api/produtos/{id}/variations/{variationId}**

* **Descrição:** Remove uma variação de produto
* **Autorização:** Requer permissão `produtos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome           | Tipo      | Descrição             |
        |----------------|-----------|-----------------------|
        | `id`           | `uuid`    | ID único do produto   |
        | `variationId`  | `uuid`    | ID único da variação  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/produtos/770e8400-e29b-41d4-a716-446655440000/variations/880e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Variação Removida**
        ```json
        {
          "success": true,
          "response": {
            "message": "Variação removida com sucesso"
          }
        }
        ```

---###
 **POST /api/produtos/{productId}/variations/{variationId}/images**

* **Descrição:** Adiciona uma imagem a uma variação de produto
* **Autorização:** Requer permissão `produtos.create`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome           | Tipo      | Descrição             |
        |----------------|-----------|-----------------------|
        | `productId`    | `uuid`    | ID único do produto   |
        | `variationId`  | `uuid`    | ID único da variação  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`multipart/form-data`)**

        | Campo        | Tipo      | Obrigatório  | Descrição                    |
        |--------------|-----------|--------------|------------------------------|
        | `imageFile`  | `file`    | Sim          | Arquivo de imagem (máx 5MB)  |
        | `imagemAlt`  | `string`  | Não          | Texto alternativo da imagem  |
        | `order`      | `number`  | Não          | Ordem da imagem              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/produtos/770e8400-e29b-41d4-a716-446655440000/variations/880e8400-e29b-41d4-a716-446655440000/images \
    -H "Authorization: Bearer <seu_token>" \
    -F 'imageFile=@imagem.jpg' \
    -F 'imagemAlt=Camiseta vermelha frente' \
    -F 'order=1'
    ```

* **Respostas:**
    * **`201 Created` - Imagem Adicionada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Imagem adicionada com sucesso",
            "data": {
              "image": {
                "id": "990e8400-e29b-41d4-a716-446655440000",
                "imagemUrl": "https://exemplo.com/uploads/imagem.jpg",
                "imagemAlt": "Camiseta vermelha frente",
                "ordem": 1
              }
            }
          }
        }
        ```

---

### **GET /api/produtos/images/{imageId}**

* **Descrição:** Obtém detalhes de uma imagem específica
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome       | Tipo      | Descrição           |
        |------------|-----------|---------------------|
        | `imageId`  | `uuid`    | ID único da imagem  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/produtos/images/990e8400-e29b-41d4-a716-446655440000
    ```

* **Respostas:**
    * **`200 OK` - Imagem Encontrada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Imagem obtida com sucesso",
            "data": {
              "image": {
                "id": "990e8400-e29b-41d4-a716-446655440000",
                "imagemUrl": "https://exemplo.com/uploads/imagem.jpg",
                "imagemAlt": "Camiseta vermelha frente",
                "ordem": 1
              }
            }
          }
        }
        ```

---

### **PATCH /api/produtos/images/{imageId}**

* **Descrição:** Atualiza uma imagem de produto
* **Autorização:** Requer permissão `produtos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome       | Tipo      | Descrição           |
        |------------|-----------|---------------------|
        | `imageId`  | `uuid`    | ID único da imagem  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`multipart/form-data`)**

        | Campo        | Tipo      | Obrigatório  | Descrição                    |
        |--------------|-----------|--------------|------------------------------|
        | `imageFile`  | `file`    | Não          | Novo arquivo de imagem       |
        | `imagemAlt`  | `string`  | Não          | Texto alternativo da imagem  |
        | `order`      | `number`  | Não          | Ordem da imagem              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/produtos/images/990e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -F 'imagemAlt=Camiseta vermelha lateral'
    ```

* **Respostas:**
    * **`200 OK` - Imagem Atualizada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Imagem atualizada com sucesso",
            "data": {
              "image": {
                "id": "990e8400-e29b-41d4-a716-446655440000",
                "imagemAlt": "Camiseta vermelha lateral"
              }
            }
          }
        }
        ```

---

### **DELETE /api/produtos/images/{imageId}**

* **Descrição:** Remove uma imagem de produto
* **Autorização:** Requer permissão `produtos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome       | Tipo      | Descrição           |
        |------------|-----------|---------------------|
        | `imageId`  | `uuid`    | ID único da imagem  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/produtos/images/990e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Imagem Removida**
        ```json
        {
          "success": true,
          "response": {
            "message": "Imagem removida com sucesso"
          }
        }
        ```

---

### **GET /api/ratings**

* **Descrição:** Lista todas as avaliações do sistema
* **Autorização:** Requer permissão `reviews.view`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/ratings \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Lista de Avaliações**
        ```json
        {
          "success": true,
          "response": {
            "message": "Avaliações obtidas com sucesso",
            "data": [
              {
                "id": "aa0e8400-e29b-41d4-a716-446655440000",
                "nota": 5,
                "comentario": "Produto excelente!",
                "usuarioId": "550e8400-e29b-41d4-a716-446655440000",
                "produtoId": "770e8400-e29b-41d4-a716-446655440000",
                "dataAvaliacao": "2024-01-16T10:00:00Z"
              }
            ]
          }
        }
        ```

---

### **GET /api/ratings/{id}**

* **Descrição:** Obtém uma avaliação específica por ID
* **Autorização:** Requer permissão `reviews.view`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição              |
        |-----------|-----------|------------------------|
        | `id`      | `uuid`    | ID único da avaliação  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/ratings/aa0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Avaliação Encontrada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Avaliação obtida com sucesso",
            "data": {
              "id": "aa0e8400-e29b-41d4-a716-446655440000",
              "nota": 5,
              "comentario": "Produto excelente!",
              "usuarioId": "550e8400-e29b-41d4-a716-446655440000",
              "produtoId": "770e8400-e29b-41d4-a716-446655440000",
              "dataAvaliacao": "2024-01-16T10:00:00Z"
            }
          }
        }
        ```

---

### **POST /api/ratings**

* **Descrição:** Cria uma nova avaliação de produto
* **Autorização:** Requer permissão `reviews.create`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo          | Tipo        | Obrigatório  | Descrição                 |
        |----------------|-------------|--------------|---------------------------|
        | `nota`         | `number`    | Sim          | Nota de 1 a 5             |
        | `usuarioId`    | `uuid`      | Sim          | ID do usuário que avalia  |
        | `comentario`   | `string`    | Não          | Comentário da avaliação   |
        | `produtoId`    | `uuid`      | Sim          | ID do produto avaliado    |
        | `dataPostada`  | `datetime`  | Sim          | Data da avaliação         |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/ratings \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "nota": 5,
      "usuarioId": "550e8400-e29b-41d4-a716-446655440000",
      "comentario": "Produto excelente, recomendo!",
      "produtoId": "770e8400-e29b-41d4-a716-446655440000",
      "dataPostada": "2024-01-16T10:00:00Z"
    }'
    ```

* **Respostas:**
    * **`201 Created` - Avaliação Criada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Avaliação criada com sucesso",
            "data": {
              "id": "bb0e8400-e29b-41d4-a716-446655440000",
              "nota": 5,
              "comentario": "Produto excelente, recomendo!",
              "dataAvaliacao": "2024-01-16T10:00:00Z"
            }
          }
        }
        ```

---

### **GET /api/ratings/products/{produtoId}**

* **Descrição:** Lista avaliações de um produto específico
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome         | Tipo      | Descrição            |
        |--------------|-----------|----------------------|
        | `produtoId`  | `uuid`    | ID único do produto  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/ratings/products/770e8400-e29b-41d4-a716-446655440000
    ```

* **Respostas:**
    * **`200 OK` - Avaliações do Produto**
        ```json
        {
          "success": true,
          "response": {
            "message": "Avaliações do produto obtidas com sucesso",
            "data": [
              {
                "id": "aa0e8400-e29b-41d4-a716-446655440000",
                "nota": 5,
                "comentario": "Produto excelente!",
                "nomeUsuario": "João Silva",
                "dataAvaliacao": "2024-01-16T10:00:00Z"
              }
            ]
          }
        }
        ```

---

### **GET /api/ratings/users/{usuarioId}**

* **Descrição:** Lista avaliações feitas por um usuário específico
* **Autorização:** Requer permissão `reviews.view`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome         | Tipo      | Descrição            |
        |--------------|-----------|----------------------|
        | `usuarioId`  | `uuid`    | ID único do usuário  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/ratings/users/550e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Avaliações do Usuário**
        ```json
        {
          "success": true,
          "response": {
            "message": "Avaliações do usuário obtidas com sucesso",
            "data": [
              {
                "id": "aa0e8400-e29b-41d4-a716-446655440000",
                "nota": 5,
                "comentario": "Produto excelente!",
                "nomeProduto": "Caneca DACC",
                "dataAvaliacao": "2024-01-16T10:00:00Z"
              }
            ]
          }
        }
        ```

---

### **PATCH /api/ratings/{id}**

* **Descrição:** Atualiza uma avaliação existente
* **Autorização:** Requer permissão `reviews.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição              |
        |-----------|-----------|------------------------|
        | `id`      | `uuid`    | ID único da avaliação  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo         | Tipo      | Obrigatório  | Descrição                |
        |---------------|-----------|--------------|--------------------------|
        | `nota`        | `number`  | Sim          | Nota de 1 a 5            |
        | `comentario`  | `string`  | Não          | Comentário da avaliação  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/ratings/aa0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "nota": 4,
      "comentario": "Produto muito bom, mas pode melhorar"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Avaliação Atualizada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Avaliação atualizada com sucesso",
            "data": {
              "id": "aa0e8400-e29b-41d4-a716-446655440000",
              "nota": 4,
              "comentario": "Produto muito bom, mas pode melhorar"
            }
          }
        }
        ```

---

### **DELETE /api/ratings/{id}**

* **Descrição:** Remove uma avaliação
* **Autorização:** Requer permissão `reviews.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição              |
        |-----------|-----------|------------------------|
        | `id`      | `uuid`    | ID único da avaliação  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/ratings/aa0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Avaliação Removida**
        ```json
        {
          "success": true,
          "response": {
            "message": "Avaliação removida com sucesso"
          }
        }
        ```

---### **
GET /api/announcements**

* **Descrição:** Lista todos os anúncios do sistema
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/announcements \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Lista de Anúncios**
        ```json
        {
          "success": true,
          "response": {
            "message": "Anúncios obtidos com sucesso",
            "data": [
              {
                "id": "cc0e8400-e29b-41d4-a716-446655440000",
                "titulo": "Novo Evento DACC",
                "conteudo": "Participe do nosso próximo evento...",
                "tipoAnuncio": "evento",
                "imagemUrl": "https://exemplo.com/anuncio.jpg",
                "ativo": true,
                "dataCriacao": "2024-01-16T12:00:00Z"
              }
            ]
          }
        }
        ```

---

### **GET /api/announcements/{id}**

* **Descrição:** Obtém um anúncio específico por ID
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do anúncio  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/announcements/cc0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Anúncio Encontrado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Anúncio obtido com sucesso",
            "data": {
              "id": "cc0e8400-e29b-41d4-a716-446655440000",
              "titulo": "Novo Evento DACC",
              "conteudo": "Participe do nosso próximo evento...",
              "tipoAnuncio": "evento",
              "imagemUrl": "https://exemplo.com/anuncio.jpg",
              "imagemAlt": "Banner do evento",
              "ativo": true,
              "autorId": "550e8400-e29b-41d4-a716-446655440000",
              "dataCriacao": "2024-01-16T12:00:00Z"
            }
          }
        }
        ```

---

### **POST /api/announcements**

* **Descrição:** Cria um novo anúncio
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo          | Tipo       | Obrigatório  | Descrição                           |
        |----------------|------------|--------------|-------------------------------------|
        | `titulo`       | `string`   | Não          | Título do anúncio                   |
        | `conteudo`     | `string`   | Não          | Conteúdo do anúncio                 |
        | `tipoAnuncio`  | `string`   | Não          | Tipo (evento, noticia, importante)  |
        | `imagemUrl`    | `string`   | Não          | URL da imagem do anúncio            |
        | `imagemAlt`    | `string`   | Não          | Texto alternativo da imagem         |
        | `ativo`        | `boolean`  | Sim          | Se o anúncio está ativo             |
        | `autorId`      | `uuid`     | Sim          | ID do autor do anúncio              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/announcements \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "titulo": "Workshop de Python",
      "conteudo": "Inscrições abertas para workshop de Python...",
      "tipoAnuncio": "evento",
      "imagemUrl": "https://exemplo.com/python.jpg",
      "imagemAlt": "Logo Python",
      "ativo": true,
      "autorId": "550e8400-e29b-41d4-a716-446655440000"
    }'
    ```

* **Respostas:**
    * **`201 Created` - Anúncio Criado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Anúncio criado com sucesso",
            "data": {
              "id": "dd0e8400-e29b-41d4-a716-446655440000",
              "titulo": "Workshop de Python",
              "tipoAnuncio": "evento",
              "ativo": true,
              "dataCriacao": "2024-01-16T13:00:00Z"
            }
          }
        }
        ```

---

### **PATCH /api/announcements/{id}**

* **Descrição:** Atualiza um anúncio existente
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do anúncio  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo          | Tipo       | Obrigatório  | Descrição            |
        |----------------|------------|--------------|----------------------|
        | `titulo`       | `string`   | Não          | Título do anúncio    |
        | `conteudo`     | `string`   | Não          | Conteúdo do anúncio  |
        | `tipoAnuncio`  | `string`   | Não          | Tipo do anúncio      |
        | `imagemUrl`    | `string`   | Não          | URL da imagem        |
        | `imagemAlt`    | `string`   | Não          | Texto alternativo    |
        | `ativo`        | `boolean`  | Não          | Status do anúncio    |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/announcements/dd0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "titulo": "Workshop de Python - Atualizado",
      "ativo": false
    }'
    ```

* **Respostas:**
    * **`200 OK` - Anúncio Atualizado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Anúncio atualizado com sucesso"
          }
        }
        ```

---

### **DELETE /api/announcements/{id}**

* **Descrição:** Remove um anúncio
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do anúncio  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/announcements/dd0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Anúncio Removido**
        ```json
        {
          "success": true,
          "response": {
            "message": "Anúncio removido com sucesso"
          }
        }
        ```

---

### **GET /api/diretores**

* **Descrição:** Lista todos os diretores
* **Autorização:** Requer permissão `faculty.view`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/diretores \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Lista de Diretores**
        ```json
        {
          "success": true,
          "response": {
            "message": "Diretores obtidos com sucesso",
            "data": [
              {
                "id": "ee0e8400-e29b-41d4-a716-446655440000",
                "nome": "Prof. Maria Silva",
                "descricao": "Diretora de Tecnologia",
                "imagemUrl": "https://exemplo.com/maria.jpg",
                "email": "maria.silva@dacc.com",
                "githubLink": "https://github.com/mariasilva",
                "linkedinLink": "https://linkedin.com/in/mariasilva",
                "diretoriaId": "ff0e8400-e29b-41d4-a716-446655440000"
              }
            ]
          }
        }
        ```

---

### **GET /api/diretores/{id}**

* **Descrição:** Obtém um diretor específico por ID
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do diretor  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/diretores/ee0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Diretor Encontrado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Diretor obtido com sucesso",
            "data": {
              "id": "ee0e8400-e29b-41d4-a716-446655440000",
              "nome": "Prof. Maria Silva",
              "descricao": "Diretora de Tecnologia com 15 anos de experiência",
              "imagemUrl": "https://exemplo.com/maria.jpg",
              "usuarioId": "550e8400-e29b-41d4-a716-446655440000",
              "diretoriaId": "ff0e8400-e29b-41d4-a716-446655440000",
              "email": "maria.silva@dacc.com",
              "githubLink": "https://github.com/mariasilva",
              "linkedinLink": "https://linkedin.com/in/mariasilva"
            }
          }
        }
        ```

---

### **POST /api/diretores**

* **Descrição:** Cria um novo diretor
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo           | Tipo      | Obrigatório  | Descrição                       |
        |-----------------|-----------|--------------|---------------------------------|
        | `id`            | `uuid`    | Sim          | ID do diretor                   |
        | `nome`          | `string`  | Não          | Nome do diretor                 |
        | `descricao`     | `string`  | Não          | Descrição/biografia do diretor  |
        | `imagemUrl`     | `string`  | Não          | URL da foto do diretor          |
        | `usuarioId`     | `uuid`    | Não          | ID do usuário associado         |
        | `diretoriaId`   | `uuid`    | Não          | ID da diretoria                 |
        | `email`         | `string`  | Não          | E-mail do diretor               |
        | `githubLink`    | `string`  | Não          | Link do GitHub                  |
        | `linkedinLink`  | `string`  | Não          | Link do LinkedIn                |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/diretores \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "id": "gg0e8400-e29b-41d4-a716-446655440000",
      "nome": "Prof. João Santos",
      "descricao": "Diretor de Pesquisa e Desenvolvimento",
      "email": "joao.santos@dacc.com",
      "githubLink": "https://github.com/joaosantos",
      "linkedinLink": "https://linkedin.com/in/joaosantos",
      "diretoriaId": "ff0e8400-e29b-41d4-a716-446655440000"
    }'
    ```

* **Respostas:**
    * **`201 Created` - Diretor Criado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Diretor criado com sucesso",
            "data": {
              "id": "gg0e8400-e29b-41d4-a716-446655440000",
              "nome": "Prof. João Santos",
              "email": "joao.santos@dacc.com"
            }
          }
        }
        ```

---

### **PATCH /api/diretores/{id}**

* **Descrição:** Atualiza um diretor existente
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do diretor  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo           | Tipo      | Obrigatório  | Descrição             |
        |-----------------|-----------|--------------|-----------------------|
        | `nome`          | `string`  | Não          | Nome do diretor       |
        | `descricao`     | `string`  | Não          | Descrição do diretor  |
        | `imagemUrl`     | `string`  | Não          | URL da imagem         |
        | `email`         | `string`  | Não          | E-mail do diretor     |
        | `githubLink`    | `string`  | Não          | Link do GitHub        |
        | `linkedinLink`  | `string`  | Não          | Link do LinkedIn      |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/diretores/gg0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "descricao": "Diretor de Pesquisa e Desenvolvimento com foco em IA"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Diretor Atualizado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Diretor atualizado com sucesso"
          }
        }
        ```

---

### **DELETE /api/diretores/{id}**

* **Descrição:** Remove um diretor
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do diretor  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/diretores/gg0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Diretor Removido**
        ```json
        {
          "success": true,
          "response": {
            "message": "Diretor removido com sucesso"
          }
        }
        ```

---#
## **GET /api/eventos**

* **Descrição:** Lista todos os eventos acadêmicos
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/eventos
    ```

* **Respostas:**
    * **`200 OK` - Lista de Eventos**
        ```json
        {
          "success": true,
          "response": {
            "message": "Eventos obtidos com sucesso",
            "data": [
              {
                "id": "hh0e8400-e29b-41d4-a716-446655440000",
                "titulo": "Workshop de React",
                "descricao": "Workshop prático sobre desenvolvimento React",
                "data": "2024-02-15T14:00:00Z",
                "tipoEvento": "workshop",
                "autorId": "550e8400-e29b-41d4-a716-446655440000",
                "textoAcao": "Inscrever-se",
                "linkAcao": "https://forms.dacc.com/react-workshop",
                "dataCriacao": "2024-01-16T14:00:00Z"
              }
            ]
          }
        }
        ```

---

### **GET /api/eventos/{id}**

* **Descrição:** Obtém um evento específico por ID
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição           |
        |-----------|-----------|---------------------|
        | `id`      | `uuid`    | ID único do evento  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/eventos/hh0e8400-e29b-41d4-a716-446655440000
    ```

* **Respostas:**
    * **`200 OK` - Evento Encontrado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Evento obtido com sucesso",
            "data": {
              "id": "hh0e8400-e29b-41d4-a716-446655440000",
              "titulo": "Workshop de React",
              "descricao": "Workshop prático sobre desenvolvimento React para iniciantes e intermediários",
              "data": "2024-02-15T14:00:00Z",
              "tipoEvento": "workshop",
              "autorId": "550e8400-e29b-41d4-a716-446655440000",
              "textoAcao": "Inscrever-se",
              "linkAcao": "https://forms.dacc.com/react-workshop",
              "dataCriacao": "2024-01-16T14:00:00Z",
              "dataAtualizacao": "2024-01-16T14:00:00Z"
            }
          }
        }
        ```

---

### **POST /api/eventos**

* **Descrição:** Cria um novo evento
* **Autorização:** Requer permissão `eventos.create`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo         | Tipo        | Obrigatório  | Descrição                              |
        |---------------|-------------|--------------|----------------------------------------|
        | `id`          | `uuid`      | Sim          | ID do evento                           |
        | `titulo`      | `string`    | Não          | Título do evento                       |
        | `descricao`   | `string`    | Não          | Descrição detalhada do evento          |
        | `data`        | `datetime`  | Não          | Data e hora do evento                  |
        | `tipoEvento`  | `string`    | Não          | Tipo (workshop, seminario, hackathon)  |
        | `autorId`     | `uuid`      | Não          | ID do autor/organizador                |
        | `textoAcao`   | `string`    | Não          | Texto do botão de ação                 |
        | `linkAcao`    | `string`    | Não          | Link para inscrição/mais informações   |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/eventos \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "id": "ii0e8400-e29b-41d4-a716-446655440000",
      "titulo": "Hackathon DACC 2024",
      "descricao": "Maior hackathon do ano com prêmios incríveis",
      "data": "2024-03-20T09:00:00Z",
      "tipoEvento": "hackathon",
      "autorId": "550e8400-e29b-41d4-a716-446655440000",
      "textoAcao": "Inscrever Equipe",
      "linkAcao": "https://hackathon.dacc.com"
    }'
    ```

* **Respostas:**
    * **`201 Created` - Evento Criado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Evento criado com sucesso",
            "data": {
              "id": "ii0e8400-e29b-41d4-a716-446655440000",
              "titulo": "Hackathon DACC 2024",
              "tipoEvento": "hackathon",
              "data": "2024-03-20T09:00:00Z",
              "dataCriacao": "2024-01-16T15:00:00Z"
            }
          }
        }
        ```

---

### **PATCH /api/eventos/{id}**

* **Descrição:** Atualiza um evento existente
* **Autorização:** Requer permissão `eventos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição           |
        |-----------|-----------|---------------------|
        | `id`      | `uuid`    | ID único do evento  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo         | Tipo        | Obrigatório  | Descrição               |
        |---------------|-------------|--------------|-------------------------|
        | `titulo`      | `string`    | Não          | Título do evento        |
        | `descricao`   | `string`    | Não          | Descrição do evento     |
        | `data`        | `datetime`  | Não          | Data e hora do evento   |
        | `tipoEvento`  | `string`    | Não          | Tipo do evento          |
        | `textoAcao`   | `string`    | Não          | Texto do botão de ação  |
        | `linkAcao`    | `string`    | Não          | Link de ação            |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/eventos/ii0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "data": "2024-03-22T09:00:00Z",
      "descricao": "Maior hackathon do ano com prêmios incríveis - Data atualizada!"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Evento Atualizado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Evento atualizado com sucesso"
          }
        }
        ```

---

### **DELETE /api/eventos/{id}**

* **Descrição:** Remove um evento
* **Autorização:** Requer permissão `eventos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição           |
        |-----------|-----------|---------------------|
        | `id`      | `uuid`    | ID único do evento  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/eventos/ii0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Evento Removido**
        ```json
        {
          "success": true,
          "response": {
            "message": "Evento removido com sucesso"
          }
        }
        ```

---

### **POST /api/eventos/{id}/register**

* **Descrição:** Registra usuário em um evento (não implementado)
* **Autorização:** Requer permissão `eventos.register`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição           |
        |-----------|-----------|---------------------|
        | `id`      | `uuid`    | ID único do evento  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/eventos/ii0e8400-e29b-41d4-a716-446655440000/register \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`501 Not Implemented` - Não Implementado**
        ```json
        {
          "success": false,
          "response": {
            "code": "NOT_IMPLEMENTED",
            "message": "Funcionalidade não implementada"
          }
        }
        ```

---

### **DELETE /api/eventos/{id}/register**

* **Descrição:** Remove registro de usuário em um evento (não implementado)
* **Autorização:** Requer permissão `eventos.register`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição           |
        |-----------|-----------|---------------------|
        | `id`      | `uuid`    | ID único do evento  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/eventos/ii0e8400-e29b-41d4-a716-446655440000/register \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`501 Not Implemented` - Não Implementado**
        ```json
        {
          "success": false,
          "response": {
            "code": "NOT_IMPLEMENTED",
            "message": "Funcionalidade não implementada"
          }
        }
        ```

---

### **GET /api/news**

* **Descrição:** Lista todas as notícias publicadas
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/news
    ```

* **Respostas:**
    * **`200 OK` - Lista de Notícias**
        ```json
        {
          "success": true,
          "response": {
            "message": "Notícias obtidas com sucesso",
            "data": [
              {
                "id": "jj0e8400-e29b-41d4-a716-446655440000",
                "titulo": "Nova Parceria com Empresa de Tecnologia",
                "descricao": "DACC firma parceria estratégica",
                "conteudo": "O DACC firmou parceria com empresa líder em tecnologia...",
                "imagemUrl": "https://exemplo.com/noticia1.jpg",
                "categoria": "parcerias",
                "autorId": "550e8400-e29b-41d4-a716-446655440000",
                "dataPublicacao": "2024-01-15T12:00:00Z",
                "dataAtualizacao": "2024-01-15T12:00:00Z"
              }
            ]
          }
        }
        ```

---

### **GET /api/news/{id}**

* **Descrição:** Obtém uma notícia específica por ID
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único da notícia  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/news/jj0e8400-e29b-41d4-a716-446655440000
    ```

* **Respostas:**
    * **`200 OK` - Notícia Encontrada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Notícia obtida com sucesso",
            "data": {
              "id": "jj0e8400-e29b-41d4-a716-446655440000",
              "titulo": "Nova Parceria com Empresa de Tecnologia",
              "descricao": "DACC firma parceria estratégica",
              "conteudo": "O DACC firmou parceria com empresa líder em tecnologia para desenvolvimento de projetos inovadores...",
              "imagemUrl": "https://exemplo.com/noticia1.jpg",
              "categoria": "parcerias",
              "autorId": "550e8400-e29b-41d4-a716-446655440000",
              "dataPublicacao": "2024-01-15T12:00:00Z",
              "dataAtualizacao": "2024-01-15T12:00:00Z"
            }
          }
        }
        ```

---

### **POST /api/news**

* **Descrição:** Cria uma nova notícia
* **Autorização:** Requer permissão `noticias.create`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo              | Tipo        | Obrigatório  | Descrição                     |
        |--------------------|-------------|--------------|-------------------------------|
        | `titulo`           | `string`    | Sim          | Título da notícia             |
        | `descricao`        | `string`    | Sim          | Descrição resumida            |
        | `conteudo`         | `string`    | Não          | Conteúdo completo da notícia  |
        | `imagemUrl`        | `string`    | Não          | URL da imagem de capa         |
        | `autorId`          | `uuid`      | Não          | ID do autor da notícia        |
        | `categoria`        | `string`    | Não          | Categoria da notícia          |
        | `dataAtualizacao`  | `datetime`  | Não          | Data de atualização           |
        | `dataPublicacao`   | `datetime`  | Não          | Data de publicação            |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/news \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "titulo": "Novo Laboratório de IA",
      "descricao": "DACC inaugura laboratório de Inteligência Artificial",
      "conteudo": "O DACC inaugurou seu novo laboratório de IA com equipamentos de última geração...",
      "categoria": "infraestrutura",
      "imagemUrl": "https://exemplo.com/lab-ia.jpg",
      "autorId": "550e8400-e29b-41d4-a716-446655440000"
    }'
    ```

* **Respostas:**
    * **`201 Created` - Notícia Criada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Notícia criada com sucesso",
            "data": {
              "id": "kk0e8400-e29b-41d4-a716-446655440000",
              "titulo": "Novo Laboratório de IA",
              "categoria": "infraestrutura",
              "dataPublicacao": "2024-01-16T18:00:00Z"
            }
          }
        }
        ```

---

### **PATCH /api/news/{id}**

* **Descrição:** Atualiza uma notícia existente
* **Autorização:** Requer permissão `noticias.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único da notícia  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo        | Tipo      | Obrigatório  | Descrição             |
        |--------------|-----------|--------------|-----------------------|
        | `titulo`     | `string`  | Não          | Título da notícia     |
        | `descricao`  | `string`  | Não          | Descrição da notícia  |
        | `conteudo`   | `string`  | Não          | Conteúdo da notícia   |
        | `imagemUrl`  | `string`  | Não          | URL da imagem         |
        | `categoria`  | `string`  | Não          | Categoria da notícia  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/news/kk0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "titulo": "Novo Laboratório de IA - Inauguração Oficial",
      "conteudo": "O DACC inaugurou oficialmente seu novo laboratório de IA com equipamentos de última geração e capacidade para 50 alunos..."
    }'
    ```

* **Respostas:**
    * **`200 OK` - Notícia Atualizada**
        ```json
        {
          "success": true,
          "response": {
            "message": "Notícia atualizada com sucesso"
          }
        }
        ```

---

### **DELETE /api/news/{id}**

* **Descrição:** Remove uma notícia
* **Autorização:** Requer permissão `noticias.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único da notícia  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/news/kk0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Notícia Removida**
        ```json
        {
          "success": true,
          "response": {
            "message": "Notícia removida com sucesso"
          }
        }
        ```

---#
## **GET /api/projects**

* **Descrição:** Lista todos os projetos acadêmicos
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/projects
    ```

* **Respostas:**
    * **`200 OK` - Lista de Projetos**
        ```json
        {
          "success": true,
          "response": {
            "message": "Projetos obtidos com sucesso",
            "data": [
              {
                "id": "ll0e8400-e29b-41d4-a716-446655440000",
                "titulo": "Sistema de Gestão Acadêmica",
                "descricao": "Desenvolvimento de sistema para gestão de notas e frequência",
                "imagemUrl": "https://exemplo.com/projeto1.jpg",
                "status": "em progresso",
                "diretoria": "Tecnologia",
                "tags": ["web", "backend", "database"],
                "dataCriacao": "2024-01-10T08:00:00Z",
                "dataAtualizacao": "2024-01-15T10:00:00Z"
              }
            ]
          }
        }
        ```

---

### **GET /api/projects/{id}**

* **Descrição:** Obtém um projeto específico por ID
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do projeto  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/projects/ll0e8400-e29b-41d4-a716-446655440000
    ```

* **Respostas:**
    * **`200 OK` - Projeto Encontrado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Projeto obtido com sucesso",
            "data": {
              "id": "ll0e8400-e29b-41d4-a716-446655440000",
              "titulo": "Sistema de Gestão Acadêmica",
              "descricao": "Desenvolvimento de sistema completo para gestão de notas, frequência e histórico acadêmico dos estudantes",
              "imagemUrl": "https://exemplo.com/projeto1.jpg",
              "status": "em progresso",
              "diretoria": "Tecnologia",
              "tags": ["web", "backend", "database", "api"],
              "dataCriacao": "2024-01-10T08:00:00Z",
              "dataAtualizacao": "2024-01-15T10:00:00Z"
            }
          }
        }
        ```

---

### **POST /api/projects**

* **Descrição:** Cria um novo projeto acadêmico
* **Autorização:** Requer permissão `projetos.create`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo        | Tipo        | Obrigatório  | Descrição                                    |
        |--------------|-------------|--------------|----------------------------------------------|
        | `titulo`     | `string`    | Não          | Título do projeto                            |
        | `descricao`  | `string`    | Não          | Descrição detalhada do projeto               |
        | `imagemUrl`  | `string`    | Não          | URL da imagem do projeto                     |
        | `status`     | `string`    | Não          | Status (planejado, em progresso, concluido)  |
        | `diretoria`  | `string`    | Não          | Diretoria responsável                        |
        | `tags`       | `string[]`  | Não          | Tags do projeto                              |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/projects \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "titulo": "App Mobile DACC",
      "descricao": "Aplicativo mobile para estudantes acessarem informações acadêmicas",
      "imagemUrl": "https://exemplo.com/app-mobile.jpg",
      "status": "planejado",
      "diretoria": "Tecnologia",
      "tags": ["mobile", "react-native", "api", "estudantes"]
    }'
    ```

* **Respostas:**
    * **`201 Created` - Projeto Criado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Projeto criado com sucesso",
            "data": {
              "id": "mm0e8400-e29b-41d4-a716-446655440000",
              "titulo": "App Mobile DACC",
              "status": "planejado",
              "diretoria": "Tecnologia",
              "dataCriacao": "2024-01-16T17:00:00Z"
            }
          }
        }
        ```

---

### **PATCH /api/projects/{id}**

* **Descrição:** Atualiza um projeto existente
* **Autorização:** Requer permissão `projetos.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do projeto  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo        | Tipo        | Obrigatório  | Descrição              |
        |--------------|-------------|--------------|------------------------|
        | `titulo`     | `string`    | Não          | Título do projeto      |
        | `descricao`  | `string`    | Não          | Descrição do projeto   |
        | `imagemUrl`  | `string`    | Não          | URL da imagem          |
        | `status`     | `string`    | Não          | Status do projeto      |
        | `diretoria`  | `string`    | Não          | Diretoria responsável  |
        | `tags`       | `string[]`  | Não          | Tags do projeto        |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/projects/mm0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "status": "em progresso",
      "descricao": "Aplicativo mobile para estudantes acessarem informações acadêmicas - Desenvolvimento iniciado"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Projeto Atualizado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Projeto atualizado com sucesso"
          }
        }
        ```

---

### **DELETE /api/projects/{id}**

* **Descrição:** Remove um projeto
* **Autorização:** Requer permissão `projetos.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `id`      | `uuid`    | ID único do projeto  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/projects/mm0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Projeto Removido**
        ```json
        {
          "success": true,
          "response": {
            "message": "Projeto removido com sucesso"
          }
        }
        ```

---

### **GET /api/posts**

* **Descrição:** Lista todos os posts do fórum
* **Autorização:** Público

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/posts
    ```

* **Respostas:**
    * **`200 OK` - Lista de Posts**
        ```json
        {
          "success": true,
          "response": {
            "message": "Posts obtidos com sucesso",
            "data": [
              {
                "id": 1,
                "titulo": "Dúvida sobre React Hooks",
                "conteudo": "Como usar useEffect corretamente?",
                "tags": ["react", "javascript", "hooks"],
                "autorId": "550e8400-e29b-41d4-a716-446655440000",
                "visualizacoes": 25,
                "respondida": false,
                "dataCriacao": "2024-01-16T10:00:00Z"
              }
            ]
          }
        }
        ```

---

### **GET /api/posts/{id}**

* **Descrição:** Obtém um post específico por ID
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição         |
        |-----------|-----------|-------------------|
        | `id`      | `number`  | ID único do post  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/posts/1
    ```

* **Respostas:**
    * **`200 OK` - Post Encontrado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Post obtido com sucesso",
            "data": {
              "id": 1,
              "titulo": "Dúvida sobre React Hooks",
              "conteudo": "Como usar useEffect corretamente? Estou tendo problemas com dependências...",
              "tags": ["react", "javascript", "hooks"],
              "autorId": "550e8400-e29b-41d4-a716-446655440000",
              "visualizacoes": 26,
              "respondida": false,
              "dataCriacao": "2024-01-16T10:00:00Z",
              "dataAtualizacao": "2024-01-16T10:00:00Z"
            }
          }
        }
        ```

---

### **POST /api/posts**

* **Descrição:** Cria um novo post no fórum
* **Autorização:** Requer permissão `forum.posts.create`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo       | Tipo        | Obrigatório  | Descrição                  |
        |-------------|-------------|--------------|----------------------------|
        | `titulo`    | `string`    | Não          | Título do post             |
        | `conteudo`  | `string`    | Não          | Conteúdo do post           |
        | `tags`      | `string[]`  | Não          | Tags relacionadas ao post  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/posts \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "titulo": "Como configurar Docker?",
      "conteudo": "Preciso de ajuda para configurar Docker no meu projeto. Alguém pode me ajudar?",
      "tags": ["docker", "devops", "configuracao"]
    }'
    ```

* **Respostas:**
    * **`201 Created` - Post Criado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Post criado com sucesso",
            "data": {
              "id": 2,
              "titulo": "Como configurar Docker?",
              "tags": ["docker", "devops", "configuracao"],
              "dataCriacao": "2024-01-16T19:00:00Z"
            }
          }
        }
        ```

---

### **PATCH /api/posts/{id}**

* **Descrição:** Atualiza um post existente
* **Autorização:** Requer permissão `forum.posts.update`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição         |
        |-----------|-----------|-------------------|
        | `id`      | `number`  | ID único do post  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo       | Tipo        | Obrigatório  | Descrição         |
        |-------------|-------------|--------------|-------------------|
        | `titulo`    | `string`    | Não          | Título do post    |
        | `conteudo`  | `string`    | Não          | Conteúdo do post  |
        | `tags`      | `string[]`  | Não          | Tags do post      |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PATCH https://api.dacc.com/api/posts/2 \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "conteudo": "Preciso de ajuda para configurar Docker no meu projeto Node.js. Alguém pode me ajudar com o Dockerfile?"
    }'
    ```

* **Respostas:**
    * **`200 OK` - Post Atualizado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Post atualizado com sucesso"
          }
        }
        ```

---

### **DELETE /api/posts/{id}**

* **Descrição:** Remove um post
* **Autorização:** Requer permissão `forum.posts.delete`

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição         |
        |-----------|-----------|-------------------|
        | `id`      | `number`  | ID único do post  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X DELETE https://api.dacc.com/api/posts/2 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Post Removido**
        ```json
        {
          "success": true,
          "response": {
            "message": "Post removido com sucesso"
          }
        }
        ```

---

### **POST /api/orders**

* **Descrição:** Cria um novo pedido com pagamento
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo         | Tipo      | Obrigatório  | Descrição                 |
        |---------------|-----------|--------------|---------------------------|
        | `orderItems`  | `array`   | Sim          | Lista de itens do pedido  |

    * **Estrutura de `orderItems`:**

        | Campo                 | Tipo      | Obrigatório  | Descrição                  |
        |-----------------------|-----------|--------------|----------------------------|
        | `productId`           | `uuid`    | Sim          | ID do produto              |
        | `productVariationId`  | `uuid`    | Sim          | ID da variação do produto  |
        | `quantity`            | `number`  | Sim          | Quantidade do item         |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/orders \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '{
      "orderItems": [
        {
          "productId": "770e8400-e29b-41d4-a716-446655440000",
          "productVariationId": "880e8400-e29b-41d4-a716-446655440000",
          "quantity": 2
        }
      ]
    }'
    ```

* **Respostas:**
    * **`201 Created` - Pedido Criado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Pedido criado com sucesso",
            "data": {
              "orderResponse": {
                "orderId": "nn0e8400-e29b-41d4-a716-446655440000",
                "total": 71.80,
                "status": "created",
                "paymentUrl": "https://mercadopago.com/checkout/v1/redirect?pref_id=123456789",
                "dataCriacao": "2024-01-16T20:00:00Z"
              }
            }
          }
        }
        ```
    * **`400 Bad Request` - Nenhum Item**
        ```json
        {
          "success": false,
          "response": {
            "code": "BAD_REQUEST",
            "message": "Nenhum item foi adicionado ao pedido"
          }
        }
        ```

---

### **GET /api/orders/{id}**

* **Descrição:** Obtém um pedido específico por ID
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição           |
        |-----------|-----------|---------------------|
        | `id`      | `uuid`    | ID único do pedido  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/orders/nn0e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Pedido Encontrado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Pedido obtido com sucesso",
            "data": {
              "orders": {
                "id": "nn0e8400-e29b-41d4-a716-446655440000",
                "usuarioId": "550e8400-e29b-41d4-a716-446655440000",
                "total": 71.80,
                "status": "approved",
                "metodoPagamento": "pix",
                "dataPedido": "2024-01-16T20:00:00Z",
                "itens": [
                  {
                    "produtoNome": "Caneca DACC",
                    "cor": "vermelho",
                    "tamanho": "G",
                    "quantidade": 2,
                    "precoUnitario": 35.90
                  }
                ]
              }
            }
          }
        }
        ```

---

### **GET /api/orders/user/{userId}**

* **Descrição:** Lista pedidos de um usuário específico
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição            |
        |-----------|-----------|----------------------|
        | `userId`  | `uuid`    | ID único do usuário  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET https://api.dacc.com/api/orders/user/550e8400-e29b-41d4-a716-446655440000 \
    -H "Authorization: Bearer <seu_token>"
    ```

* **Respostas:**
    * **`200 OK` - Pedidos do Usuário**
        ```json
        {
          "success": true,
          "response": {
            "message": "Pedidos obtidos com sucesso",
            "data": [
              {
                "id": "nn0e8400-e29b-41d4-a716-446655440000",
                "total": 71.80,
                "status": "approved",
                "dataPedido": "2024-01-16T20:00:00Z"
              }
            ]
          }
        }
        ```

---

### **PUT /api/orders/{id}/status**

* **Descrição:** Atualiza o status de um pedido
* **Autorização:** Requer autenticação

* **Parâmetros da Requisição:**
    * **Path**

        | Nome      | Tipo      | Descrição           |
        |-----------|-----------|---------------------|
        | `id`      | `uuid`    | ID único do pedido  |
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`application/json`)**

        | Campo     | Tipo      | Obrigatório  | Descrição                                                                 |
        |-----------|-----------|--------------|---------------------------------------------------------------------------|
        | `status`  | `string`  | Sim          | Novo status (created, pending, approved, rejected, delivered, cancelled)  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X PUT https://api.dacc.com/api/orders/nn0e8400-e29b-41d4-a716-446655440000/status \
    -H "Authorization: Bearer <seu_token>" \
    -H "Content-Type: application/json" \
    -d '"delivered"'
    ```

* **Respostas:**
    * **`200 OK` - Status Atualizado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Status atualizado com sucesso",
            "data": {
              "orderId": "nn0e8400-e29b-41d4-a716-446655440000",
              "status": "delivered"
            }
          }
        }
        ```

---

### **POST /api/orders/webhook**

* **Descrição:** Processa webhook do MercadoPago para atualização de pagamentos
* **Autorização:** Público (validação por assinatura)

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome           | Tipo      | Descrição                          |
        |----------------|-----------|------------------------------------|
        | `x-signature`  | `string`  | Assinatura do webhook MercadoPago  |
    * **Body (`application/json`)**

        | Campo     | Tipo      | Obrigatório  | Descrição                 |
        |-----------|-----------|--------------|---------------------------|
        | `type`    | `string`  | Sim          | Tipo do evento (payment)  |
        | `data`    | `object`  | Sim          | Dados do evento           |

    * **Estrutura de `data`:**

        | Campo     | Tipo      | Obrigatório  | Descrição                       |
        |-----------|-----------|--------------|---------------------------------|
        | `id`      | `string`  | Sim          | ID do pagamento no MercadoPago  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/orders/webhook \
    -H "x-signature: ts=1234567890,v1=signature_hash" \
    -H "Content-Type: application/json" \
    -d '{
      "type": "payment",
      "data": {
        "id": "123456789"
      }
    }'
    ```

* **Respostas:**
    * **`200 OK` - Webhook Processado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Pagamento realizado com sucesso"
          }
        }
        ```
    * **`400 Bad Request` - Webhook Inválido**
        ```json
        {
          "success": false,
          "response": {
            "code": "INVALID_WEBHOOK",
            "message": "Falha na validação da assinatura do webhook"
          }
        }
        ```

---#
## **GET /api/payments/success**

* **Descrição:** Página de sucesso do pagamento (callback MercadoPago)
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Query**

        | Nome                  | Tipo      | Padrão    | Descrição                        |
        |-----------------------|-----------|-----------|----------------------------------|
        | `external_reference`  | `string`  | -         | Referência externa do pagamento  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "https://api.dacc.com/api/payments/success?external_reference=order_123"
    ```

* **Respostas:**
    * **`200 OK` - Pagamento Bem-sucedido**
        ```json
        {
          "success": true,
          "response": {
            "message": "Pagamento realizado com sucesso",
            "data": "order_123"
          }
        }
        ```

---

### **GET /api/payments/failure**

* **Descrição:** Página de falha do pagamento (callback MercadoPago)
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Query**

        | Nome                  | Tipo      | Padrão    | Descrição                        |
        |-----------------------|-----------|-----------|----------------------------------|
        | `external_reference`  | `string`  | -         | Referência externa do pagamento  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "https://api.dacc.com/api/payments/failure?external_reference=order_123"
    ```

* **Respostas:**
    * **`200 OK` - Pagamento Falhou**
        ```json
        {
          "success": true,
          "response": {
            "message": "Pagamento falhou. Tente novamente",
            "data": "order_123"
          }
        }
        ```

---

### **GET /api/payments/pending**

* **Descrição:** Página de pagamento pendente (callback MercadoPago)
* **Autorização:** Público

* **Parâmetros da Requisição:**
    * **Query**

        | Nome                  | Tipo      | Padrão    | Descrição                        |
        |-----------------------|-----------|-----------|----------------------------------|
        | `external_reference`  | `string`  | -         | Referência externa do pagamento  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X GET "https://api.dacc.com/api/payments/pending?external_reference=order_123"
    ```

* **Respostas:**
    * **`200 OK` - Pagamento Pendente**
        ```json
        {
          "success": true,
          "response": {
            "message": "Pagamento pendente. Aguarde a confirmação",
            "data": "order_123"
          }
        }
        ```

---

### **POST /api/filestorage/uploadImage**

* **Descrição:** Faz upload de uma imagem
* **Autorização:** Requer cargo `administrador`

* **Parâmetros da Requisição:**
    * **Headers**

        | Nome             | Tipo      | Descrição     |
        |------------------|-----------|---------------|
        | `Authorization`  | `string`  | Bearer token  |
    * **Body (`multipart/form-data`)**

        | Campo     | Tipo      | Obrigatório  | Descrição                    |
        |-----------|-----------|--------------|------------------------------|
        | `file`    | `file`    | Sim          | Arquivo de imagem (máx 5MB)  |

* **Exemplo de Requisição (cURL):**
    ```shell
    curl -X POST https://api.dacc.com/api/filestorage/uploadImage \
    -H "Authorization: Bearer <seu_token>" \
    -F 'file=@imagem.jpg'
    ```

* **Respostas:**
    * **`200 OK` - Upload Realizado**
        ```json
        {
          "success": true,
          "response": {
            "message": "Upload realizado com sucesso",
            "data": {
              "url": "https://exemplo.com/uploads/imagem_123.jpg"
            }
          }
        }
        ```
    * **`400 Bad Request` - Arquivo Inválido**
        ```json
        {
          "success": false,
          "response": {
            "code": "BAD_REQUEST",
            "message": "Nenhum arquivo foi enviado"
          }
        }
        ```
    * **`413 Payload Too Large` - Arquivo Muito Grande**
        ```json
        {
          "success": false,
          "response": {
            "code": "CONTENT_TOO_LARGE",
            "message": "Arquivo maior que 5MB"
          }
        }
        ```

---

## Códigos de Erro

### Códigos de Autenticação (4xx)
- **`AUTH_TOKEN_INVALID`** (401): Token JWT inválido
- **`AUTH_TOKEN_EXPIRED`** (401): Token JWT expirado
- **`INVALID_CREDENTIALS`** (401): Credenciais inválidas
- **`AUTH_INSUFFICIENT_PERMISSIONS`** (403): Permissões insuficientes

### Códigos de Validação (4xx)
- **`VALIDATION_ERROR`** (400): Erro de validação dos dados
- **`BAD_REQUEST`** (400): Dados inválidos na requisição
- **`RESOURCE_NOT_FOUND`** (404): Recurso não encontrado
- **`RESOURCE_ALREADY_EXISTS`** (409): Recurso já existe

### Códigos Específicos do Domínio (4xx)
- **`ACCOUNT_INACTIVE`** (400): Conta desativada
- **`INSUFFICIENT_STOCK`** (400): Estoque insuficiente
- **`PRODUCT_OUT_OF_STOCK`** (400): Produto fora de estoque
- **`CART_ITEM_NOT_FOUND`** (404): Item não encontrado no carrinho
- **`EVENT_FULL`** (400): Evento lotado
- **`REGISTRATION_CLOSED`** (400): Inscrições encerradas
- **`CONTENT_TOO_LARGE`** (413): Arquivo maior que 5MB
- **`PAYMENT_FAILED`** (400): Falha no processamento do pagamento
- **`INVALID_WEBHOOK`** (400): Webhook inválido

### Códigos Técnicos (5xx)
- **`INTERNAL_SERVER_ERROR`** (500): Erro interno do servidor
- **`RATE_LIMIT_EXCEEDED`** (429): Limite de requisições excedido

---

## Sistema de Permissões

A API utiliza um sistema de permissões granular baseado em cargos:

### Cargos Disponíveis
- **aluno**: Permissões básicas de visualização e interação
- **diretor**: Permissões de gestão de conteúdo e projetos
- **administrador**: Todas as permissões do sistema

## Considerações Técnicas

### Paginação
Endpoints que retornam listas suportam paginação via query parameters:
- `page`: Número da página (padrão: 1)
- `limit`: Itens por página (padrão: 16, máximo: 100)

### Upload de Arquivos
- Tamanho máximo: 5MB por arquivo
- Formatos suportados: Imagens (processadas via SixLabors.ImageSharp)
- Diretório de armazenamento: `wwwroot/uploads/`

### Integração MercadoPago
- Ambiente: Sandbox (teste)
- Métodos de pagamento: PIX e vendas físicas
- Webhooks: Validação por assinatura obrigatória
- Callbacks: URLs de sucesso, falha e pendência configuráveis

### Banco de Dados
- PostgreSQL hospedado na Neon
- Suporte a transações para operações críticas
- Procedures para operações complexas (ex: remoção de estoque múltiplo)

---

*Documentação gerada automaticamente em 05/08/2025*