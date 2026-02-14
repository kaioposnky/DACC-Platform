-- Arquivo: seed_data.sql
-- Objetivo: Popular o banco de dados com dados fictícios para desenvolvimento e teste.
-- Nota: Execute este script APÓS rodar o sqlcode.sql, pois ele depende das tabelas e dos domínios (tipos, cores, etc.) já inseridos.

BEGIN;

DO $$
    DECLARE
        -- Variáveis para armazenar IDs gerados
        v_admin_id UUID;
        v_diretor_id UUID;
        v_aluno_id UUID;
        v_aluno2_id UUID;
        v_diretoria_id UUID;
        v_produto_camisa_id UUID;
        v_produto_adesivo_id UUID;
        v_post_id UUID;
        v_comentario_id UUID;
        v_noticia_id UUID;
        v_noticia2_id UUID;
        v_tag_infra_id UUID;
        v_tag_evento_id UUID;
        v_anuncio_id UUID;
        v_cupom_id UUID;
        v_pedido_id UUID;

        -- Variáveis para buscar IDs de tabelas de domínio existentes
        v_sub_camisetas_id UUID;
        v_sub_adesivos_id UUID;
        v_cor_preto_id UUID;
        v_cor_branco_id UUID;
        v_tam_m_id UUID;
        v_tam_g_id UUID;

    BEGIN
        ---------------------------------------------------------------------------
        -- 1. CRIAÇÃO DE USUÁRIOS
        ---------------------------------------------------------------------------

        -- Criar Administrador
        INSERT INTO usuario (nome, sobrenome, email, ra, curso, telefone, senha_hash, cargo, ativo)
        VALUES ('Admin', 'Sistema', 'admin@dacc.com', '000001', 'Engenharia de Software', '11999999999', 'hash_senha_segura_123', 'administrador', TRUE)
        RETURNING id INTO v_admin_id;

        -- Criar Diretor
        INSERT INTO usuario (nome, sobrenome, email, ra, curso, telefone, senha_hash, cargo, ativo)
        VALUES ('Carlos', 'Diretor', 'carlos@dacc.com', '000002', 'Ciência da Computação', '11988888888', 'hash_senha_segura_123', 'diretor', TRUE)
        RETURNING id INTO v_diretor_id;

        -- Criar Aluno 1
        INSERT INTO usuario (nome, sobrenome, email, ra, curso, telefone, senha_hash, cargo, ativo)
        VALUES ('João', 'Silva', 'joao.silva@aluno.com', '2024001', 'Engenharia de Software', '11977777777', 'hash_senha_segura_123', 'aluno', TRUE)
        RETURNING id INTO v_aluno_id;

        -- Criar Aluno 2
        INSERT INTO usuario (nome, sobrenome, email, ra, curso, telefone, senha_hash, cargo, ativo)
        VALUES ('Maria', 'Souza', 'maria.souza@aluno.com', '2024002', 'Sistemas de Informação', '11966666666', 'hash_senha_segura_123', 'aluno', TRUE)
        RETURNING id INTO v_aluno2_id;

        RAISE NOTICE 'Usuários criados com sucesso.';

        ---------------------------------------------------------------------------
        -- 2. DIRETORIA E MEMBROS (FACULTY)
        ---------------------------------------------------------------------------

        -- Criar Diretoria de Tecnologia
        INSERT INTO diretoria (nome, descricao)
        VALUES ('Tecnologia e Inovação', 'Responsável pela manutenção da plataforma e novos projetos técnicos.')
        RETURNING id INTO v_diretoria_id;

        -- Adicionar o usuário Diretor à tabela de Diretores (Faculty)
        INSERT INTO diretores (nome, titulo, cargo, especializacao, imagem_url, email, linkedin, usuario_id)
        VALUES ('Carlos Diretor', 'MSc.', 'Diretor de Tech', 'Fullstack Development', 'https://gerenciador.fei.edu.br/Content/Arquivos/logo_fei_color-01.svg', 'carlos@dacc.com', 'linkedin.com/in/carlos', v_diretor_id);

        ---------------------------------------------------------------------------
        -- 3. CONTEÚDO (NOTÍCIAS, EVENTOS, PROJETOS)
        ---------------------------------------------------------------------------

        -- Tags de Notícia
        INSERT INTO noticia_tag (nome) VALUES ('Infraestrutura') RETURNING id INTO v_tag_infra_id;
        INSERT INTO noticia_tag (nome) VALUES ('Eventos') RETURNING id INTO v_tag_evento_id;
        INSERT INTO noticia_tag (nome) VALUES ('Academia');

        -- Notícia 1
        INSERT INTO noticia (titulo, descricao, conteudo, imagem_url, imagem_alt, autor_id, categoria, data_publicacao)
        VALUES
            ('Bem-vindo ao Novo Portal DACC', 'Lançamento oficial da nova plataforma.', 'Estamos muito felizes em anunciar o lançamento do novo portal...', 'https://gerenciador.fei.edu.br/Content/Arquivos/logo_fei_color-01.svg', 'Banner de lançamento', v_admin_id, 'geral', NOW())
        RETURNING id INTO v_noticia_id;

        -- Notícia 2
        INSERT INTO noticia (titulo, descricao, conteudo, imagem_url, imagem_alt, autor_id, categoria, data_publicacao)
        VALUES
            ('Hackathon 2024 Confirmado', 'Preparem suas equipes para o maior evento do ano.', 'O Hackathon ocorrerá em novembro...', 'https://gerenciador.fei.edu.br/Content/Arquivos/logo_fei_color-01.svg', 'Banner Hackathon', v_diretor_id, 'tecnologia', NOW())
        RETURNING id INTO v_noticia2_id;

        -- Relacionar Tags
        INSERT INTO noticia_tags_relacao (noticia_id, tag_id) VALUES (v_noticia_id, v_tag_infra_id), (v_noticia2_id, v_tag_evento_id);

        -- Evento
        INSERT INTO evento (titulo, descricao, data, tipo_evento, autor_id, texto_acao, link_acao)
        VALUES ('Workshop de React', 'Aprenda os fundamentos de ReactJS.', NOW() + INTERVAL '7 days', 'workshop', v_diretor_id, 'Inscrever-se', 'https://forms.gle/exemplo');

        -- Projeto
        INSERT INTO projeto (titulo, descricao, status, progresso, texto_conclusao, diretoria, tags)
        VALUES ('DACC Platform', 'Desenvolvimento do portal do diretório.', 'em progresso', 75, 'Lançamento Beta', 'Tecnologia e Inovação', ARRAY['C#', 'React', 'SQL']);

        ---------------------------------------------------------------------------
        -- 4. LOJA (PRODUTOS, VARIAÇÕES, CUPONS)
        ---------------------------------------------------------------------------

        -- Buscar IDs auxiliares (assumindo que o sqlcode.sql já inseriu estes dados)
        SELECT id INTO v_sub_camisetas_id FROM produto_subcategoria WHERE nome = 'camisetas';
        SELECT id INTO v_sub_adesivos_id FROM produto_subcategoria WHERE nome = 'adesivos';
        SELECT id INTO v_cor_preto_id FROM produto_cor WHERE nome = 'azul'; -- Usando azul como exemplo se preto não existir (mas existe no sqlcode)
        SELECT id INTO v_cor_preto_id FROM produto_cor WHERE nome = 'preto';
        SELECT id INTO v_cor_branco_id FROM produto_cor WHERE nome = 'branco';
        SELECT id INTO v_tam_m_id FROM produto_tamanho WHERE nome = 'M';
        SELECT id INTO v_tam_g_id FROM produto_tamanho WHERE nome = 'G';

        -- Produto 1: Camiseta
        INSERT INTO produto (nome, descricao, preco, preco_original, subcategoria_id, ativo, descricao_detalhada, destaque)
        VALUES (
                   'Camiseta DACC 2024',
                   'Camiseta oficial do diretório, 100% algodão.',
                   45.00,
                   60.00,
                   v_sub_camisetas_id,
                   TRUE,
                   'Nossa camiseta é feita com algodão premium, garantindo conforto e durabilidade. A estampa é feita em silk-screen de alta qualidade.',
                   TRUE
               )
        RETURNING id INTO v_produto_camisa_id;

        -- Especificações da Camiseta
        INSERT INTO produto_especificacao (produto_id, nome, valor)
        VALUES
            (v_produto_camisa_id, 'Material', '100% Algodão'),
            (v_produto_camisa_id, 'Estampa', 'Silk-Screen'),
            (v_produto_camisa_id, 'Modelagem', 'Unissex');

        -- Informações de Envio da Camiseta
        INSERT INTO produto_informacao_envio (produto_id, frete_gratis, dias_estimados, custo_envio, politica_devolucao, garantia)
        VALUES (v_produto_camisa_id, FALSE, 5, 15.00, 'Troca em até 7 dias', '30 dias contra defeitos');

        -- Perfeito Para (Camiseta)
        INSERT INTO produto_perfeito_para (produto_id, ocasiao)
        VALUES
            (v_produto_camisa_id, 'Uso diário'),
            (v_produto_camisa_id, 'Eventos da faculdade'),
            (v_produto_camisa_id, 'Presentear amigos');

        -- Variações da Camiseta
        -- Preto M
        INSERT INTO produto_variacao (produto_id, cor_id, tamanho_id, estoque, sku, ordem)
        VALUES (v_produto_camisa_id, v_cor_preto_id, v_tam_m_id, 50, 'CAM-24-BLK-M', 1);

        -- Preto G
        INSERT INTO produto_variacao (produto_id, cor_id, tamanho_id, estoque, sku, ordem)
        VALUES (v_produto_camisa_id, v_cor_preto_id, v_tam_g_id, 30, 'CAM-24-BLK-G', 2);

        -- Imagens da Camiseta
        INSERT INTO produto_imagem (produto_variacao_id, imagem_url, imagem_alt, ordem)
        SELECT id, 'https://gerenciador.fei.edu.br/Content/Arquivos/logo_fei_color-01.svg', 'Logo FEI', 2
        FROM produto_variacao WHERE sku = 'CAM-24-BLK-M';

        INSERT INTO produto_imagem (produto_variacao_id, imagem_url, imagem_alt, ordem)
        SELECT id, 'https://gerenciador.fei.edu.br/Content/Arquivos/logo_fei_color-01.svg', 'Camiseta Preta Frente', 1
        FROM produto_variacao WHERE sku = 'CAM-24-BLK-M';

        -- Produto 2: Adesivo
        INSERT INTO produto (nome, descricao, preco, subcategoria_id, ativo, descricao_detalhada, destaque)
        VALUES (
                   'Pack de Adesivos Dev',
                   'Pacote com 5 adesivos variados.',
                   10.00,
                   v_sub_adesivos_id,
                   TRUE,
                   'Adesivos de vinil resistentes à água, perfeitos para personalizar seu notebook.',
                   FALSE
               )
        RETURNING id INTO v_produto_adesivo_id;

        -- Variação Adesivo
        INSERT INTO produto_variacao (produto_id, cor_id, tamanho_id, estoque, sku)
        VALUES (v_produto_adesivo_id, v_cor_branco_id, v_tam_m_id, 100, 'ADE-DEV-01');

        -- Cupom de Desconto
        INSERT INTO cupom (codigo, tipo_desconto, valor, data_expiracao, limite_uso, ativo)
        VALUES ('BEMVINDO10', 'porcentagem', 10.00, NOW() + INTERVAL '30 days', 100, TRUE)
        RETURNING id INTO v_cupom_id;

        ---------------------------------------------------------------------------
        -- 5. FÓRUM
        ---------------------------------------------------------------------------

        -- Post criado pelo Aluno 1
        INSERT INTO post (titulo, conteudo, autor_id, tags, respondida, visualizacoes, data_criacao, data_atualizacao)
        VALUES ('Dúvida sobre o Hackathon', 'Alguém sabe se precisa ter equipe formada ou posso entrar sozinho?', v_aluno_id, ARRAY['eventos', 'duvida'], FALSE, 15, NOW(), NOW())
        RETURNING id INTO v_post_id;

        -- Comentário do Diretor
        INSERT INTO comentario (post_id, autor_id, conteudo, aceito, upvotes, downvotes, data_criacao, data_atualizacao)
        VALUES (v_post_id, v_diretor_id, 'Olá João! Você pode se inscrever individualmente e nós montaremos as equipes no dia.', TRUE, 5, 0, NOW(), NOW())
        RETURNING id INTO v_comentario_id;

        -- Relacionamento Tabela Auxiliar Comentários
        INSERT INTO comentarios_post (post_id, comentario_id) VALUES (v_post_id, v_comentario_id);

        -- Voto no Post (Aluno 2 curtiu)
        INSERT INTO votacao_post (post_id, usuario_id, voto)
        VALUES (v_post_id, v_aluno2_id, TRUE);

        ---------------------------------------------------------------------------
        -- 6. AVALIAÇÕES DE PRODUTOS
        ---------------------------------------------------------------------------

        INSERT INTO avaliacao (usuario_id, produto_id, nota, comentario, ativo)
        VALUES (v_aluno_id, v_produto_camisa_id, 5.0, 'A camiseta é de ótima qualidade!', TRUE);

        ---------------------------------------------------------------------------
        -- 7. ANÚNCIOS (Banners da Home)
        ---------------------------------------------------------------------------

        INSERT INTO anuncio (
            titulo, conteudo, tipo_anuncio, botao_primario_texto, botao_primario_link,
            botao_secundario_texto, botao_secundario_link, imagem_url, imagem_alt, ativo, autor_id
        )
        VALUES (
                   'Semana da Tecnologia', 'Venha participar de palestras incríveis.', 'evento',
                   'Ver Cronograma', '/eventos', 'Saber Mais', '/sobre',
                   'https://gerenciador.fei.edu.br/Content/Arquivos/logo_fei_color-01.svg',
                   'Banner Tech Week', TRUE, v_diretor_id
               )
        RETURNING id INTO v_anuncio_id;

        -- Detalhes do Anúncio (Carousel ou Itens extras)
        INSERT INTO anuncio_detalhe (anuncio_id, ordem, conteudo, imagem_url)
        VALUES
            (v_anuncio_id, 1, 'Palestra sobre IA com especialistas.', 'https://img.url/ia.webp'),
            (v_anuncio_id, 2, 'Workshop de Segurança da Informação.', 'https://img.url/sec.webp');

        ---------------------------------------------------------------------------
        -- 8. PEDIDOS (HISTÓRICO)
        ---------------------------------------------------------------------------

        INSERT INTO pedido (usuario_id, status_pedido, total_pedido, cupom_id, metodo_pagamento)
        VALUES (v_aluno_id, 'approved', 40.50, v_cupom_id, 'pix')
        RETURNING id INTO v_pedido_id;

        -- Itens do Pedido
        INSERT INTO item_pedido (pedido_id, produto_id, produto_variacao_id, quantidade, preco_unitario)
        SELECT v_pedido_id, v_produto_camisa_id, id, 1, 45.00
        FROM produto_variacao WHERE sku = 'CAM-24-BLK-M';

        RAISE NOTICE 'Seed de dados concluído com sucesso!';
    END $$;

COMMIT;
