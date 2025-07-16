-- 1. Tabela: Tipos de Usuário
DROP TABLE IF EXISTS tipos_usuario;
CREATE TABLE tipos_usuario
(
    id   SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

DROP TABLE IF EXISTS tipos_anuncio;
CREATE TABLE tipos_anuncio
(
    id   SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

DROP TABLE IF EXISTS tipos_evento;
CREATE TABLE tipos_evento
(
    id   SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

DROP TABLE IF EXISTS tipos_progresso;
CREATE TABLE tipos_progresso
(
    id   SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- 2. Tabela: Usuários
DROP TABLE IF EXISTS usuarios;
CREATE TABLE usuarios
(
    id               SERIAL PRIMARY KEY,
    nome             VARCHAR(100)        NOT NULL,
    email            VARCHAR(150) UNIQUE NOT NULL,
    telefone         VARCHAR(15)         NOT NULL,
    senha_hash       VARCHAR(200)        NOT NULL,
    imagem_url       VARCHAR(255)        NOT NULL,
    ativo            BOOLEAN   DEFAULT FALSE,
    data_criacao     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    tipo_usuario_id  INT REFERENCES tipos_usuario (id)
);

DROP TABLE IF EXISTS posts;
CREATE TABLE posts
(
    id               SERIAL PRIMARY KEY,
    titulo           VARCHAR(50) NOT NULL,
    conteudo         TEXT        NOT NULL,
    autor_id         INT REFERENCES usuarios (id),
    tags             VARCHAR(255)[] NOT NULL,
    respondida       BOOLEAN     NOT NULL,
    visualizacoes    INT         NOT NULL,
    upvotes          INT         NOT NULL,
    downvotes        INT         NOT NULL,
    data_criacao     TIMESTAMP   NOT NULL,
    data_atualizacao TIMESTAMP   NOT NULL
);

DROP TABLE IF EXISTS comentarios_post;
CREATE TABLE comentarios_post
(
    id            SERIAL PRIMARY KEY,
    post_id       INT REFERENCES posts (id),
    comentario_id INT REFERENCES comentario (id)
);

DROP TABLE IF EXISTS comentario;
CREATE TABLE comentario
(
    id               SERIAL PRIMARY KEY,
    post_id          INT REFERENCES posts (id),
    autor_id         INT REFERENCES usuarios (id),
    conteudo         TEXT      NOT NULL,
    aceito           BOOLEAN   NOT NULL,
    upvotes          INT       NOT NULL,
    downvotes        INT       NOT NULL,
    data_criacao     TIMESTAMP NOT NULL,
    data_atualizacao TIMESTAMP NOT NULL
);

DROP TABLE IF EXISTS anuncio;
CREATE TABLE anuncio
(
    id               SERIAL PRIMARY KEY,
    titulo           VARCHAR(50) NOT NULL,
    conteudo         TEXT        NOT NULL,
    tipo_anuncio_id  INT REFERENCES tipos_anuncio (id),
    ativo            BOOLEAN     NOT NULL DEFAULT FALSE,
    autor_id         INT REFERENCES usuarios (id),
    data_criacao     TIMESTAMP   NOT NULL,
    data_atualizacao TIMESTAMP   NOT NULL
);

DROP TABLE IF EXISTS evento;
CREATE TABLE evento
(
    id               SERIAL PRIMARY KEY,
    titulo           VARCHAR(50)  NOT NULL,
    descricao        TEXT         NOT NULL,
    data             TIMESTAMP    NOT NULL,
    localizacao      VARCHAR(100) NOT NULL,
    tipo_evento_id   INT REFERENCES tipos_evento (id),
    capacidade       INT          NOT NULL,
    registrados      INT          NOT NULL,
    autor_id         INT REFERENCES usuarios (id),
    data_criacao     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 3. Tabela: Categorias
DROP TABLE IF EXISTS categorias;
CREATE TABLE categorias
(
    id      SERIAL PRIMARY KEY,
    nome    VARCHAR(100) NOT NULL,
    subtipo VARCHAR(100)
);

-- Tabela de Cores disponíveis
DROP TABLE IF EXISTS cores;
CREATE TABLE cores
(
    id   SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- Tabela de Tamanhos disponíveis
DROP TABLE IF EXISTS tamanhos;
CREATE TABLE tamanhos
(
    id        SERIAL PRIMARY KEY,
    nome      VARCHAR(10) NOT NULL UNIQUE,
    descricao VARCHAR(50)
);

-- Tabela para variações do produto
DROP TABLE IF EXISTS produtos_variacoes;
CREATE TABLE produtos_variacoes
(
    id         SERIAL PRIMARY KEY,
    produto_id INT REFERENCES produtos (id),
    cor_id     INT REFERENCES cores (id),
    tamanho_id INT REFERENCES tamanhos (id),
    estoque    INT NOT NULL DEFAULT 0,
    padrao     BOOLEAN      DEFAULT FALSE,
    UNIQUE (produto_id, cor_id, tamanho_id)
);


-- 4. Tabela: Produtos
DROP TABLE IF EXISTS produtos;
CREATE TABLE produtos
(
    id               SERIAL PRIMARY KEY,
    nome             VARCHAR(150)   NOT NULL,
    descricao        TEXT           NOT NULL,
    preco            NUMERIC(10, 2) NOT NULL,
    desconto         NUMERIC(10, 2),
    genero           BOOLEAN,
    data_criacao     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    categoria_id     INT REFERENCES categorias (id)
);

-- 5. Tabela: Imagens do Produto
DROP TABLE IF EXISTS imagens_produto;
CREATE TABLE imagens_produto
(
    id         SERIAL PRIMARY KEY,
    produto_id INT REFERENCES produtos (id),
    cor_id REFERENCES cores (id),
    imagem_url VARCHAR(255) NOT NULL,
    ordem      INT          NOT NULL DEFAULT 0
);

-- 6. Tabela: Status de Carrinho
DROP TABLE IF EXISTS status_carrinho;
CREATE TABLE status_carrinho
(
    id   SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- 7. Tabela: Carrinhos
DROP TABLE IF EXISTS carrinhos;
CREATE TABLE carrinhos
(
    id                 SERIAL PRIMARY KEY,
    usuario_id         INT REFERENCES usuarios (id),
    status_carrinho_id INT REFERENCES status_carrinho (id),
    data_criacao       TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 8. Tabela: Itens do Carrinho
DROP TABLE IF EXISTS itens_carrinho;
CREATE TABLE itens_carrinho
(
    id          SERIAL PRIMARY KEY,
    carrinho_id INT REFERENCES carrinhos (id),
    produto_id  INT REFERENCES produtos (id),
    quantidade  INT NOT NULL
);

-- 9. Tabela: Status de Pedido
DROP TABLE IF EXISTS status_pedido;
CREATE TABLE status_pedido
(
    id   SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- 10. Tabela: Métodos de Pagamento
DROP TABLE IF EXISTS metodos_pagamento;
CREATE TABLE metodos_pagamento
(
    id   SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- 11. Tabela: Pedidos
DROP TABLE IF EXISTS pedidos;
CREATE TABLE pedidos
(
    id                  SERIAL PRIMARY KEY,
    usuario_id          INT REFERENCES usuarios (id),
    data_pedido         TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    status_pedido_id    INT REFERENCES status_pedido (id),
    metodo_pagamento_id INT REFERENCES metodos_pagamento (id)
);

-- 12. Tabela: Itens do Pedido
DROP TABLE IF EXISTS itens_pedido;
CREATE TABLE itens_pedido
(
    id         SERIAL PRIMARY KEY,
    pedido_id  INT REFERENCES pedidos (id),
    produto_id INT REFERENCES produtos (id),
    quantidade INT NOT NULL
);

-- 13. Tabela: Avaliações
DROP TABLE IF EXISTS avaliacoes;
CREATE TABLE avaliacoes
(
    id             SERIAL PRIMARY KEY,
    usuario_id     INT REFERENCES usuarios (id),
    produto_id     INT REFERENCES produtos (id),
    nota           INT CHECK (nota BETWEEN 1 AND 5),
    comentario     TEXT,
    data_avaliacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 14. Tabela: Diretorias
DROP TABLE IF EXISTS diretorias;
CREATE TABLE diretorias
(
    id        SERIAL PRIMARY KEY,
    nome      VARCHAR(100) NOT NULL,
    descricao TEXT
);

-- 15. Tabela: Notícias
DROP TABLE IF EXISTS noticias;
CREATE TABLE noticias
(
    id              SERIAL PRIMARY KEY,
    titulo          VARCHAR(200) NOT NULL,
    conteudo        TEXT,
    imagem_url      VARCHAR(255),
    data_publicacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 16. Tabela: Projetos
drop table if exists projetos;
CREATE TABLE projetos
(
    id               SERIAL PRIMARY KEY,
    titulo           VARCHAR(200) NOT NULL,
    descricao        TEXT         NOT NULL,
    imagem_url       VARCHAR(255) NOT NULL,
    status REFERENCES tipos_progresso (id),
    lider_id         INT REFERENCES usuarios (id),
    diretoria_id     INT REFERENCES diretorias (id),
    membros_id       INT REFERENCES usuarios (id)[],
    data_criacao     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tipos de Usuário
INSERT INTO tipos_usuario (nome)
VALUES ('aluno'),
       ('visitante'),
       ('diretor'),
       ('administrador');

-- Tipos de Anuncio
INSERT INTO tipos_anuncio (nome)
VALUES ('evento'),
       ('noticia'),
       ('importante');

-- Tipos de Evento
INSERT INTO tipos_evento (nome)
VALUES ('workshop'),
       ('seminario'),
       ('hackathon');

-- Tipos de Progresso
INSERT INTO tipos_progresso (nome)
VALUES ('planejado'),
       ('em progresso'),
       ('concluido');

-- Status de Carrinho
INSERT INTO status_carrinho (nome)
VALUES ('ativo'),
       ('finalizado'),
       ('abandonado');

-- Status de Pedido
INSERT INTO status_pedido (nome)
VALUES ('aguardando pagamento'),
       ('pago'),
       ('enviado'),
       ('entregue'),
       ('cancelado');

-- Métodos de Pagamento
INSERT INTO metodos_pagamento (nome)
VALUES ('venda física'),
       ('pix');