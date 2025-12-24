-- Tabela: Tipos de Usuário
-- Armazena os diferentes tipos de usuários do sistema
DROP TABLE IF EXISTS tipos_usuario CASCADE;
CREATE TABLE tipos_usuario
(
    id   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- Tabela: Tipos de Anúncio
-- Define os diferentes tipos de anúncios que podem ser publicados
DROP TABLE IF EXISTS tipos_anuncio CASCADE;
CREATE TABLE tipos_anuncio
(
    id   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- Tabela: Tipos de Evento
-- Categoriza os diferentes tipos de eventos que podem ser organizados
DROP TABLE IF EXISTS tipos_evento CASCADE;
CREATE TABLE tipos_evento
(
    id   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- Tabela: Tipos de Progresso
-- Define os diferentes estados de progresso para projetos
DROP TABLE IF EXISTS tipos_progresso CASCADE;
CREATE TABLE tipos_progresso
(
    id   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- Tabela: Categorias de Notícia
-- Armazena as categorias disponíveis para classificar notícias
DROP TABLE IF EXISTS categorias_noticia CASCADE;
CREATE TABLE categorias_noticia
(
    id   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- -- Tabela: Usuários
-- -- Armazena informações dos usuários do sistema
DROP TABLE IF EXISTS usuario CASCADE;
CREATE TABLE usuario
(
    id                   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome                 VARCHAR(100)        NOT NULL,
    sobrenome            VARCHAR(100)        NOT NULL,
    email                VARCHAR(150) UNIQUE NOT NULL,
    ra                   VARCHAR(15) UNIQUE,
    curso                VARCHAR(200)        NOT NULL,
    telefone             VARCHAR(15),
    senha_hash           VARCHAR(200)        NOT NULL,
    imagem_url           VARCHAR(255),
    ativo                BOOLEAN   DEFAULT TRUE,
    newsletter_subscriber BOOLEAN   DEFAULT FALSE,
    cargo                VARCHAR(50) REFERENCES tipos_usuario (nome),
    data_criacao         TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao     TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela: Posts
-- Armazena posts do fórum
DROP TABLE IF EXISTS post CASCADE;
CREATE TABLE post
(
    id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    titulo           VARCHAR(50)    NOT NULL,
    conteudo         TEXT           NOT NULL,
    autor_id         UUID REFERENCES usuario (id),
    tags             VARCHAR(255)[] NOT NULL,
    respondida       BOOLEAN        NOT NULL,
    visualizacoes    INT            NOT NULL,
    data_criacao     TIMESTAMP      NOT NULL,
    data_atualizacao TIMESTAMP      NOT NULL
);

-- Tabela: Votação de Posts
-- Registra os votos dos usuários em posts
DROP TABLE IF EXISTS votacao_post CASCADE;
CREATE TABLE votacao_post
(
    post_id    UUID REFERENCES post (id),
    usuario_id UUID REFERENCES usuario (id),
    voto       BOOLEAN NOT NULL,
    PRIMARY KEY (post_id, usuario_id)
);

-- Tabela: Comentários
-- Armazena comentários feitos em posts
DROP TABLE IF EXISTS comentario CASCADE;
CREATE TABLE comentario
(
    id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    post_id          UUID REFERENCES post (id),
    autor_id         UUID REFERENCES usuario (id),
    conteudo         TEXT      NOT NULL,
    aceito           BOOLEAN   NOT NULL,
    upvotes          INT       NOT NULL,
    downvotes        INT       NOT NULL,
    data_criacao     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela: Comentários de Posts
-- Relaciona comentários com seus respectivos posts
DROP TABLE IF EXISTS comentarios_post CASCADE;
CREATE TABLE comentarios_post
(
    id            UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    post_id       UUID REFERENCES post (id),
    comentario_id UUID REFERENCES comentario (id)
);

-- Tabela: Anúncios
-- Armazena anúncios do sistema
DROP TABLE IF EXISTS anuncio CASCADE;
CREATE TABLE anuncio
(
    id                     UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    titulo                 VARCHAR(50) NOT NULL,
    conteudo               TEXT        NOT NULL,
    tipo_anuncio           VARCHAR(50) REFERENCES tipos_anuncio (nome) NOT NULL,
    botao_primario_texto   VARCHAR(20) NOT NULL,
    botao_primario_link    VARCHAR(255) NOT NULL,
    botao_secundario_texto VARCHAR(20) NOT NULL,
    botao_secundario_link  VARCHAR(255) NOT NULL,
    imagem_url             VARCHAR(255) NOT NULL,
    imagem_alt             VARCHAR(100) NOT NULL,
    ativo                  BOOLEAN     NOT NULL DEFAULT FALSE,
    autor_id               UUID REFERENCES usuario (id),
    data_criacao           TIMESTAMP DEFAULT  CURRENT_TIMESTAMP NOT NULL,
    data_atualizacao       TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL
);

-- Tabela: Detalhes de Anúncios
-- Armazena detalhes adicionais para anúncios
DROP TABLE IF EXISTS anuncio_detalhe CASCADE;
CREATE TABLE anuncio_detalhe
(
    id         UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    anuncio_id UUID REFERENCES anuncio (id),
    ordem      INT DEFAULT 0,
    imagem_url VARCHAR(255),
    conteudo   VARCHAR(255),
    UNIQUE (anuncio_id, ordem)
);

-- Tabela: Eventos
-- Armazena informações sobre eventos
DROP TABLE IF EXISTS evento CASCADE;
CREATE TABLE evento
(
    id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    titulo           VARCHAR(50) NOT NULL,
    descricao        TEXT        NOT NULL,
    data             TIMESTAMP   NOT NULL,
    tipo_evento      VARCHAR(50) REFERENCES tipos_evento (nome),
    autor_id         UUID REFERENCES usuario (id),
    texto_acao       VARCHAR(20) NOT NULL,
    link_acao        VARCHAR(255) NOT NULL,
    data_criacao     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Reordenar para que diretoria venha antes de diretores
-- Tabela: Diretorias
-- Armazena as diferentes diretorias da organização
DROP TABLE IF EXISTS diretoria CASCADE;
CREATE TABLE diretoria
(
    id        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome      VARCHAR(100) UNIQUE NOT NULL,
    descricao TEXT
);

-- Tabela: Diretores
-- Armazena informações sobre os diretores (Faculty no frontend)
DROP TABLE IF EXISTS diretores CASCADE;
CREATE TABLE diretores
(
    id             UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome           VARCHAR(100) NOT NULL,
    titulo         VARCHAR(50) NOT NULL,
    cargo          VARCHAR(100) NOT NULL,
    especializacao VARCHAR(200) NOT NULL,
    imagem_url     VARCHAR(255) NOT NULL,
    email          VARCHAR(150),
    linkedin       VARCHAR(255),
    github         VARCHAR(255),
    usuario_id     UUID REFERENCES usuario (id),
    data_criacao   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela: Cores
-- Armazena cores disponíveis para produtos
DROP TABLE IF EXISTS produto_cor CASCADE;
CREATE TABLE produto_cor
(
    id   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- Tabela: Tamanhos
-- Armazena tamanhos disponíveis para produtos
DROP TABLE IF EXISTS produto_tamanho CASCADE;
CREATE TABLE produto_tamanho
(
    id        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome      VARCHAR(10) NOT NULL UNIQUE,
    descricao VARCHAR(50)
);

-- Tabela: Categorias
-- Armazena categorias para produtos
DROP TABLE IF EXISTS produto_categoria CASCADE;
CREATE TABLE produto_categoria
(
    id      UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome    VARCHAR(100) UNIQUE NOT NULL
);

-- Tabela: Subcategoria
-- Armazena as subcategorias de produtos
DROP TABLE IF EXISTS produto_subcategoria CASCADE;
CREATE TABLE produto_subcategoria(
                                     id      UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                                     nome    VARCHAR(100) UNIQUE NOT NULL,
                                     categoria_id UUID REFERENCES produto_categoria (id)
);

-- Tabela: Produtos
-- Armazena informações sobre produtos
DROP TABLE IF EXISTS produto CASCADE;
CREATE TABLE produto
(
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome                VARCHAR(150) NOT NULL,
    descricao           TEXT         NOT NULL,
    descricao_detalhada TEXT,
    destaque            BOOLEAN      DEFAULT FALSE,
    preco               NUMERIC(10, 2) NOT NULL,
    preco_original      NUMERIC(10, 2),
    subcategoria_id     UUID REFERENCES produto_subcategoria (id),
    ativo               BOOLEAN      NOT NULL DEFAULT TRUE,
    data_criacao        TIMESTAMP             DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao    TIMESTAMP             DEFAULT CURRENT_TIMESTAMP
);

-- Tabela: Especificações de Produto
-- Armazena especificações técnicas dos produtos
DROP TABLE IF EXISTS produto_especificacao CASCADE;
CREATE TABLE produto_especificacao
(
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    produto_id  UUID REFERENCES produto (id) ON DELETE CASCADE,
    nome        VARCHAR(100) NOT NULL,
    valor       VARCHAR(255) NOT NULL,
    UNIQUE (produto_id, nome)
);

-- Tabela: Informações de Envio de Produto
-- Armazena informações sobre envio e garantia
DROP TABLE IF EXISTS produto_informacao_envio CASCADE;
CREATE TABLE produto_informacao_envio
(
    id                 UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    produto_id         UUID REFERENCES produto (id) ON DELETE CASCADE UNIQUE,
    frete_gratis       BOOLEAN DEFAULT FALSE,
    dias_estimados     VARCHAR(50) NOT NULL,
    custo_envio        NUMERIC(10, 2),
    politica_devolucao VARCHAR(255) NOT NULL,
    garantia           VARCHAR(100)
);

-- Tabela: Perfeito Para (Ocasiões de uso do produto)
-- Armazena as ocasiões para as quais o produto é perfeito
DROP TABLE IF EXISTS produto_perfeito_para CASCADE;
CREATE TABLE produto_perfeito_para
(
    id          UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    produto_id  UUID REFERENCES produto (id) ON DELETE CASCADE,
    ocasiao     VARCHAR(100) NOT NULL,
    UNIQUE (produto_id, ocasiao)
);

-- Tabela: Variações de Produto
-- Armazena as variações de cada produto
DROP TABLE IF EXISTS produto_variacao CASCADE;
CREATE TABLE produto_variacao
(
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    produto_id          UUID REFERENCES produto (id) ON DELETE CASCADE,
    cor_id              UUID REFERENCES produto_cor (id),
    tamanho_id          UUID REFERENCES produto_tamanho (id),
    estoque             INT            NOT NULL DEFAULT 0,
    sku                 VARCHAR(100) UNIQUE,
    ordem               INT          NOT NULL DEFAULT 0,
    data_criacao        TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao    TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (produto_id, cor_id, tamanho_id)
);

-- Tabela: Imagens de Produto
-- Armazena imagens relacionadas a produtos
DROP TABLE IF EXISTS produto_imagem CASCADE;
CREATE TABLE produto_imagem
(
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    produto_variacao_id UUID REFERENCES produto_variacao (id) ON DELETE CASCADE,
    imagem_url          VARCHAR(255) NOT NULL,
    imagem_alt          VARCHAR(100),
    ordem               INT          NOT NULL DEFAULT 0
);

-- Tabela: Reservas de produto
-- Cria uma reserva de quantidade de produto para ser alterado no pedido
DROP TABLE IF EXISTS reserva_produto CASCADE;
CREATE TABLE reserva_produto (
                                 id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                                 produto_variacao_id UUID REFERENCES produto_variacao(id),
                                 pedido_id UUID REFERENCES produto(id),
                                 quantidade INT NOT NULL,
                                 ativo BOOLEAN DEFAULT true,
                                 data_expira TIMESTAMP NOT NULL,
                                 data_criacao TIMESTAMP DEFAULT NOW()
);

-- Tabela: Status de Pedido
-- Define os diferentes estados possíveis para um pedido
DROP TABLE IF EXISTS status_pedido CASCADE;
CREATE TABLE status_pedido
(
    id   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- Tabela: Métodos de Pagamento
-- Armazena os métodos de pagamento disponíveis
DROP TABLE IF EXISTS metodo_pagamento CASCADE;
CREATE TABLE metodo_pagamento
(
    id   UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(50) NOT NULL UNIQUE
);

-- Tabela: Pedidos
-- Armazena informações sobre pedidos
DROP TABLE IF EXISTS pedido CASCADE;
CREATE TABLE pedido
(
    id                       UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id               UUID REFERENCES usuario (id),
    data_pedido              TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    status_pedido            VARCHAR(50) REFERENCES status_pedido (nome),
    mercadopago_pagamento_id BIGINT NULL,
    preference_id            VARCHAR(100) NULL,
    metodo_pagamento         VARCHAR(50) REFERENCES metodo_pagamento (nome) NULL,
    total_pedido             NUMERIC(10, 2) NOT NULL
);

-- Tabela: Itens do Pedido
-- Armazena itens individuais de um pedido
DROP TABLE IF EXISTS item_pedido CASCADE;
CREATE TABLE item_pedido
(
    id                  UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    pedido_id           UUID REFERENCES pedido (id) ON DELETE CASCADE,
    produto_id          UUID REFERENCES produto (id),
    produto_variacao_id UUID REFERENCES produto_variacao (id),
    quantidade          INT            NOT NULL,
    preco_unitario      NUMERIC(10, 2) NOT NULL
);

-- Tabela: Avaliações
-- Armazena avaliações de produtos feitas pelos usuários
DROP TABLE IF EXISTS avaliacao CASCADE;
CREATE TABLE avaliacao
(
    id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id       UUID REFERENCES usuario (id),
    produto_id       UUID REFERENCES produto (id) ON DELETE CASCADE,
    nota             DECIMAL(2,1) CHECK (nota BETWEEN 0.5 AND 5.0),
    comentario       TEXT,
    ativo            BOOLEAN   DEFAULT TRUE,
    data_avaliacao   TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela: Notícias
-- Armazena notícias publicadas no sistema
DROP TABLE IF EXISTS noticia CASCADE;
CREATE TABLE noticia
(
    id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    titulo           VARCHAR(200) NOT NULL,
    descricao        VARCHAR(255) NOT NULL,
    conteudo         TEXT,
    imagem_url       VARCHAR(255),
    imagem_alt       VARCHAR(255),
    autor_id         UUID REFERENCES usuario (id),
    categoria        VARCHAR(50) REFERENCES categorias_noticia (nome),
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_publicacao  TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela: Projetos
-- Armazena informações sobre projetos
DROP TABLE IF EXISTS projeto CASCADE;
CREATE TABLE projeto
(
    id               UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    titulo           VARCHAR(200) NOT NULL,
    descricao        TEXT         NOT NULL,
    imagem_url       VARCHAR(255),
    status           VARCHAR(50) REFERENCES tipos_progresso (nome),
    progresso        INT DEFAULT 0,
    texto_conclusao  VARCHAR(100) NOT NULL,
    diretoria        VARCHAR(100) REFERENCES diretoria (nome),
    tags             VARCHAR(20)[],
    data_criacao     TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela: UsuarioTokens
-- Armazena o AcessToken e o RefreshToken do usuário
DROP TABLE IF EXISTS usuario_tokens CASCADE;
CREATE TABLE usuario_tokens
(
    usuario_id    UUID REFERENCES usuario (id),
    access_token  VARCHAR(10000) NOT NULL,
    refresh_token VARCHAR(10000) NOT NULL,
    UNIQUE (usuario_id)
);

-- Tabela: Permissões
-- Armazena as permissões disponíveis no sistema
DROP TABLE IF EXISTS permissoes CASCADE;
CREATE TABLE permissoes
(
    id        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome      VARCHAR(100) NOT NULL UNIQUE,
    descricao TEXT
);

-- Tabela: Permissões por Tipo de Usuário
-- Relaciona permissões com tipos de usuário
DROP TABLE IF EXISTS role_permissoes CASCADE;
CREATE TABLE role_permissoes
(
    tipo_usuario VARCHAR(50) REFERENCES tipos_usuario (nome) ON DELETE CASCADE,
    permissao    VARCHAR(100) REFERENCES permissoes (nome) ON DELETE CASCADE,
    PRIMARY KEY (tipo_usuario, permissao)
);

-- Cores disponívies de produtos
INSERT INTO produto_cor(nome)
VALUES ('azul'),
       ('vermelho'),
       ('branco'),
       ('preto');

-- Tamanhos disponíveis de produtos
INSERT INTO produto_tamanho(nome)
VALUES ('P'),
       ('M'),
       ('G'),
       ('GG');

-- Tipos de Usuário
INSERT INTO tipos_usuario (nome)
VALUES ('aluno'),
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

-- Categorias de Notícia
INSERT INTO categorias_noticia (nome)
VALUES ('geral'),
       ('tecnologia'),
       ('carreira'),
       ('academico');

-- Status de Pedido
INSERT INTO status_pedido (nome)
VALUES ('created'),
       ('pending'),
       ('approved'),
       ('rejected'),
       ('delivered'),
       ('cancelled');

-- Métodos de Pagamento
INSERT INTO metodo_pagamento (nome)
VALUES ('venda física'),
       ('pix');

-- Categorias de Produtos
INSERT INTO produto_categoria (nome)
VALUES ('roupas'), ('outros');

-- Subcategorias de Produtos
INSERT INTO produto_subcategoria (nome, categoria_id)
VALUES ( 'camisetas', (SELECT id FROM produto_categoria WHERE nome = 'roupas')),
       ( 'moletom', (SELECT id FROM produto_categoria WHERE nome = 'roupas')),
       ( 'canecas', (SELECT id FROM produto_categoria WHERE nome = 'outros')),
       ( 'adesivos', (SELECT id FROM produto_categoria WHERE nome = 'outros')),
       ( 'acessorios', (SELECT id FROM produto_categoria WHERE nome = 'outros'));

-- Permissões
INSERT INTO permissoes (nome, descricao)
VALUES
    -- Permissões de Usuários
    ('users.view', 'Visualizar um usuário'),
    ('users.viewall', 'Visualizar todos os usuários'),
    ('users.create', 'Criar novos usuários'),
    ('users.update', 'Atualizar informações de usuários'),
    ('users.delete', 'Deletar usuários'),

    -- Permissões de Notícias
    ('noticias.view', 'Visualizar notícias'),
    ('noticias.create', 'Criar novas notícias'),
    ('noticias.update', 'Atualizar notícias'),
    ('noticias.delete', 'Deletar notícias'),

    -- Permissões de Projetos
    ('projetos.view', 'Visualizar projetos'),
    ('projetos.create', 'Criar novos projetos'),
    ('projetos.update', 'Atualizar projetos'),
    ('projetos.delete', 'Deletar projetos'),
    ('projetos.members.add', 'Adicionar membros a um projeto'),
    ('projetos.members.remove', 'Remover membros de um projeto'),

    -- Permissões de Produtos
    ('produtos.view', 'Visualizar produtos da loja'),
    ('produtos.create', 'Criar novos produtos na loja'),
    ('produtos.update', 'Atualizar produtos na loja'),
    ('produtos.delete', 'Deletar produtos na loja'),

    -- Permissões de Eventos
    ('eventos.view', 'Visualizar eventos'),
    ('eventos.create', 'Criar novos eventos'),
    ('eventos.update', 'Atualizar eventos'),
    ('eventos.delete', 'Deletar eventos'),
    ('eventos.register', 'Registrar-se em um evento'),

    -- Permissões do Forum (Posts e Comentários)
    ('forum.posts.view', 'Visualizar posts e comentários do fórum'),
    ('forum.posts.create', 'Criar posts no fórum'),
    ('forum.posts.update', 'Atualizar próprios posts no fórum'),
    ('forum.posts.delete', 'Deletar próprios posts no fórum'),
    ('forum.posts.vote', 'Votar em posts'),
    ('forum.comments.create', 'Criar comentários em posts'),
    ('forum.comments.update', 'Atualizar próprios comentários'),
    ('forum.comments.delete', 'Deletar próprios comentários'),
    ('forum.comments.vote', 'Votar em comentários'),
    ('forum.comments.accept', 'Marcar um comentário como resposta aceita em seu próprio post'),
    ('forum.admin.posts.update', 'Atualizar qualquer post no fórum'),
    ('forum.admin.posts.delete', 'Deletar qualquer post no fórum'),
    ('forum.admin.comments.update', 'Atualizar qualquer comentário no fórum'),
    ('forum.admin.comments.delete', 'Deletar qualquer comentário no fórum'),

    -- Permissões de Diretorias/Professores
    ('faculty.view', 'Visualizar professores'),
    ('faculty.create', 'Criar novos professores'),
    ('faculty.update', 'Atualizar professores'),
    ('faculty.delete', 'Deletar professores'),

    -- Permissões de Avaliações de Produtos
    ('reviews.view', 'Visualizar avaliações de um produto'),
    ('reviews.create', 'Criar uma avaliação para um produto'),
    ('reviews.update','Atualizar uma avaliação para um produto'),
    ('reviews.delete', 'Deletar uma avaliação para um produto'),

    -- Permissões de Envio de arquivos no BackEnd
    ('filestorage.uploadimage', 'Enviar e salvar uma imagem no backend'),

    -- Permissões do Carrinho
    ('cart.view', 'Visualizar o carrinho'),
    ('cart.items.add', 'Adicionar itens ao carrinho'),
    ('cart.items.update', 'Atualizar itens no carrinho'),
    ('cart.items.remove', 'Remover itens do carrinho'),
    ('cart.clear', 'Limpar o carrinho');

-- Atribuindo Permissões aos Roles (Tipos de Usuário)
TRUNCATE TABLE role_permissoes;
DO
$$
    DECLARE
        permissao_rec RECORD;
    BEGIN
        -- Permissões do Aluno
        INSERT INTO role_permissoes (tipo_usuario, permissao)
        SELECT 'aluno', p.nome
        FROM permissoes p
        WHERE p.nome IN (
            -- Usuários
            'users.view', 'users.update', 'users.refreshtoken', 'users.logout',

            -- Notícias
            'noticias.view',

            -- Projetos
            'projetos.view',

            -- Produtos
            'produtos.view',

            -- Eventos
            'eventos.view', 'eventos.register',

            -- Fórum
            'forum.posts.view',
            'forum.posts.create', 'forum.posts.update', 'forum.posts.delete', 'forum.posts.vote',
            'forum.comments.create', 'forum.comments.update', 'forum.comments.delete', 'forum.comments.vote', 'forum.comments.accept',

            -- Professores
            'faculty.view',

            -- Avaliações
            'reviews.view', 'reviews.create',

            -- Carrinho
            'cart.view', 'cart.items.add', 'cart.items.update', 'cart.items.remove', 'cart.clear'
        );

        -- Permissões do Diretor
        -- 1. Herdar todas as permissões do Aluno
        INSERT INTO role_permissoes (tipo_usuario, permissao)
        SELECT 'diretor', permissao
        FROM role_permissoes
        WHERE tipo_usuario = 'aluno';

        -- 2. Adicionar permissões específicas de Diretor (Moderador)
        INSERT INTO role_permissoes (tipo_usuario, permissao)
        SELECT 'diretor', p.nome
        FROM permissoes p
        WHERE p.nome IN (
            -- Usuários
            'users.viewall',

            -- Notícias
            'noticias.create', 'noticias.update', 'noticias.delete',

            -- Projetos
            'projetos.create', 'projetos.update', 'projetos.delete', 'projetos.members.add', 'projetos.members.remove',

            -- Produtos
            'produtos.create', 'produtos.update', 'produtos.delete',

            -- Eventos
            'eventos.create', 'eventos.update', 'eventos.delete',

            -- Fórum (Admin)
            'forum.admin.posts.update', 'forum.admin.posts.delete',
            'forum.admin.comments.update', 'forum.admin.comments.delete',

            -- Professores
            'faculty.create', 'faculty.update', 'faculty.delete',

            -- Avaliações
            'reviews.update', 'reviews.delete',

            -- Arquivos
            'filestorage.uploadimage'
        )
        ON CONFLICT DO NOTHING;

        -- Permissões do Administrador (todas)
        FOR permissao_rec IN SELECT nome FROM permissoes
            LOOP
                INSERT INTO role_permissoes (tipo_usuario, permissao)
                VALUES ('administrador', permissao_rec.nome)
                ON CONFLICT DO NOTHING;
            END LOOP;
    END
$$;


--- PROCEDURES

-- Função para verificar se há estoque suficiente para múltiplos produtos
CREATE OR REPLACE FUNCTION check_multiple_products_stock(
    VariationIds UUID[],
    Quantities INTEGER[]
) RETURNS TABLE(success BOOLEAN, error_message TEXT) AS $$
DECLARE
    i INTEGER;
    current_stock INTEGER;
    product_name TEXT;
    product_cor TEXT;
    product_tamanho TEXT;
BEGIN
    -- Verificar se todos têm estoque suficiente
    FOR i IN 1..array_length(VariationIds, 1) LOOP
            SELECT pv.estoque, p.nome, pc.nome, pt.nome
            INTO current_stock, product_name, product_cor, product_tamanho
            FROM produto_variacao pv
                     INNER JOIN produto p ON pv.produto_id = p.id
                     INNER JOIN produto_cor pc ON pv.cor_id = pc.id
                     INNER JOIN produto_tamanho pt ON pv.tamanho_id = pt.id
            WHERE pv.id = VariationIds[i];

            IF current_stock IS NULL THEN
                RETURN QUERY SELECT FALSE, 'Produto com ID ' || VariationIds[i] || ' não encontrado';
                RETURN;
            END IF;

            IF current_stock < Quantities[i] THEN
                RETURN QUERY SELECT FALSE,
                                    'Produto ' || product_name || 'cor ' || product_cor || ' tamanho ' || product_tamanho
                                        || ' sem estoque suficiente. Disponível: ' ||
                                    current_stock || ', Solicitado: ' || Quantities[i];
                RETURN;
            END IF;
        END LOOP;

    -- Se chegou até aqui, todos têm estoque suficiente
    RETURN QUERY SELECT TRUE, ''::TEXT;
END;
$$ LANGUAGE plpgsql;

-- Função para remover estoque de múltiplos produtos (sem verificação)
CREATE OR REPLACE FUNCTION remove_multiple_products_stock_direct(
    VariationIds UUID[],
    Quantities INTEGER[]
) RETURNS TABLE(success BOOLEAN, error_message TEXT) AS $$
DECLARE
    i INTEGER;
    rows_affected INTEGER;
BEGIN
    -- Atualizar o estoque de todos os produtos
    FOR i IN 1..array_length(VariationIds, 1) LOOP
            UPDATE produto_variacao
            SET estoque = estoque - Quantities[i]
            WHERE id = VariationIds[i];

            GET DIAGNOSTICS rows_affected = ROW_COUNT;

            IF rows_affected = 0 THEN
                RETURN QUERY SELECT FALSE, 'Falha ao atualizar estoque do produto com ID ' || VariationIds[i];
                RETURN;
            END IF;
        END LOOP;

    RETURN QUERY SELECT TRUE, ''::TEXT;
END;
$$ LANGUAGE plpgsql;