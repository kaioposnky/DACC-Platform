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
DROP TABLE IF EXISTS usuario;
CREATE TABLE usuario
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

DROP TABLE IF EXISTS post;
CREATE TABLE post
(
    id               SERIAL PRIMARY KEY,
    titulo           VARCHAR(50) NOT NULL,
    conteudo         TEXT        NOT NULL,
    autor_id         INT REFERENCES usuario (id),
    tags             VARCHAR(255)[] NOT NULL,
    respondida       BOOLEAN     NOT NULL,
    visualizacoes    INT         NOT NULL,
    data_criacao     TIMESTAMP   NOT NULL,
    data_atualizacao TIMESTAMP   NOT NULL
);

DROP TABLE IF EXISTS votacao_post_usuario;
CREATE TABLE votacao_post(
     post_id INT REFERENCES post (id),
     user_id INT REFERENCES usuario(id),
     voto  BOOLEAN NOT NULL,
)

DROP TABLE IF EXISTS comentario;
CREATE TABLE comentario
(
    id               SERIAL PRIMARY KEY,
    post_id          INT REFERENCES post (id),
    autor_id         INT REFERENCES usuario (id),
    conteudo         TEXT      NOT NULL,
    aceito           BOOLEAN   NOT NULL,
    upvotes          INT       NOT NULL,
    downvotes        INT       NOT NULL,
    data_criacao     TIMESTAMP NOT NULL,
    data_atualizacao TIMESTAMP NOT NULL
);

DROP TABLE IF EXISTS comentarios_post;
CREATE TABLE comentarios_post
(
    id            SERIAL PRIMARY KEY,
    post_id       INT REFERENCES post (id),
    comentario_id INT REFERENCES comentario (id)
);

DROP TABLE IF EXISTS anuncio;
CREATE TABLE anuncio
(
    id               SERIAL PRIMARY KEY,
    titulo           VARCHAR(50) NOT NULL,
    conteudo         TEXT        NOT NULL,
    tipo_anuncio_id  INT REFERENCES tipos_anuncio (id),
    ativo            BOOLEAN     NOT NULL DEFAULT FALSE,
    autor_id         INT REFERENCES usuario (id),
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
    autor_id         INT REFERENCES usuario (id),
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

-- 4. Tabela: Produtos
DROP TABLE IF EXISTS produto;
CREATE TABLE produto
(
    id               SERIAL PRIMARY KEY,
    nome             VARCHAR(150)   NOT NULL,
    descricao        TEXT           NOT NULL,
    preco            NUMERIC(10, 2) NOT NULL,
    preco_original   NUMERIC(10, 2),
    genero           BOOLEAN,
    data_criacao     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    categoria_id     INT REFERENCES categorias (id)
);

-- Tabela para variações do produto
DROP TABLE IF EXISTS produto_variacao;
CREATE TABLE produto_variacao
(
    id         SERIAL PRIMARY KEY,
    produto_id INT REFERENCES produto (id),
    cor_id     INT REFERENCES cores (id),
    tamanho_id INT REFERENCES tamanhos (id),
    estoque    INT NOT NULL DEFAULT 0,
    padrao     BOOLEAN      DEFAULT FALSE,
    UNIQUE (produto_id, cor_id, tamanho_id)
);

-- 5. Tabela: Imagens do Produto
DROP TABLE IF EXISTS imagem_produto;
CREATE TABLE imagem_produto
(
    id         SERIAL PRIMARY KEY,
    produto_id INT REFERENCES produto (id),
    cor_id     INT REFERENCES cores (id),
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
DROP TABLE IF EXISTS carrinho;
CREATE TABLE carrinho
(
    id                 SERIAL PRIMARY KEY,
    usuario_id         INT REFERENCES usuario (id),
    status_carrinho_id INT REFERENCES status_carrinho (id),
    data_criacao       TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 8. Tabela: Itens do Carrinho
DROP TABLE IF EXISTS item_carrinho;
CREATE TABLE item_carrinho
(
    id          SERIAL PRIMARY KEY,
    carrinho_id INT REFERENCES carrinho (id),
    produto_id  INT REFERENCES produto (id),
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
DROP TABLE IF EXISTS metodo_pagamento;
CREATE TABLE metodo_pagamento
(
    id   SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- 11. Tabela: Pedidos
DROP TABLE IF EXISTS pedido;
CREATE TABLE pedido
(
    id                  SERIAL PRIMARY KEY,
    usuario_id          INT REFERENCES usuario (id),
    data_pedido         TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    status_pedido_id    INT REFERENCES status_pedido (id),
    metodo_pagamento_id INT REFERENCES metodo_pagamento (id)
);

-- 12. Tabela: Itens do Pedido
DROP TABLE IF EXISTS item_pedido;
CREATE TABLE item_pedido
(
    id         SERIAL PRIMARY KEY,
    pedido_id  INT REFERENCES pedido (id),
    produto_id INT REFERENCES produto (id),
    quantidade INT NOT NULL
);

-- 13. Tabela: Avaliações
DROP TABLE IF EXISTS avaliacao;
CREATE TABLE avaliacao
(
    id             SERIAL PRIMARY KEY,
    usuario_id     INT REFERENCES usuario (id),
    produto_id     INT REFERENCES produto (id),
    nota           INT CHECK (nota BETWEEN 1 AND 5),
    comentario     TEXT,
    data_avaliacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 14. Tabela: Diretorias
DROP TABLE IF EXISTS diretoria;
CREATE TABLE diretoria
(
    id        SERIAL PRIMARY KEY,
    nome      VARCHAR(100) NOT NULL,
    descricao TEXT
);

-- 15. Tabela: Notícias
DROP TABLE IF EXISTS noticia;
CREATE TABLE noticia
(
    id              SERIAL PRIMARY KEY,
    titulo          VARCHAR(200) NOT NULL,
    descricao       VARCHAR(255) NOT NULL,
    conteudo        TEXT,
    imagem_url      VARCHAR(255),
    data_publicacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 16. Tabela: Projetos
DROP TABLE IF EXISTS projeto;
CREATE TABLE projeto
(
    id               SERIAL PRIMARY KEY,
    titulo           VARCHAR(200) NOT NULL,
    descricao        TEXT         NOT NULL,
    imagem_url       VARCHAR(255) NOT NULL,
    status_id        INT REFERENCES tipos_progresso (id),
    lider_id         INT REFERENCES usuario (id),
    diretoria_id     INT REFERENCES diretoria (id),
    membros_id       INT[] NOT NULL,
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
INSERT INTO metodo_pagamento (nome)
VALUES ('venda física'),
       ('pix');

-- Categorias de Produtos
INSERT INTO categorias (nome, subtipo)
VALUES ('roupas', 'camisetas'),
       ('roupas', 'moletom'),
       ('outros', 'canecas'),
       ('outros', 'adesivos'),
       ('outros', 'acessorios');
