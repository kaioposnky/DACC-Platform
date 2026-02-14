"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { SocialIcons, FooterSection } from '@/components/molecules';

interface FooterProps {
  className?: string;
}

export const Footer = ({ className = '' }: FooterProps) => {
  const quickLinks = [
    { label: 'Inicio', href: '/' },
    { label: 'Sobre', href: '/sobre' },
    { label: 'Projetos', href: '/sobre#projects' },
    { label: 'Eventos', href: '/inicio#eventos' },
    { label: 'Notícias', href: '/noticias' },
    // { label: 'Fórum', href: '/forum' }
  ];

  const resources = [
    { label: 'Portal do Aluno', href: 'https://interage.fei.org.br/secureserver/portal', target: '_blank' },
    { label: 'Moodle', href: 'https://moodle.fei.edu.br/login/index.php', target: '_blank' },
  ];

  return (
    <footer className={`bg-slate-800 text-white ${className}`}>
      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        {/* Main Footer Content */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8 lg:gap-12">
          {/* Brand Section */}
          <motion.div
            className="lg:col-span-1"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6 }}
          >
            <Typography variant="h3" color="white" className="font-bold mb-4">
              Coruja Overflow
            </Typography>
            <Typography variant="body" className="text-white mb-6 leading-relaxed">
              Empoderando estudantes de ciência da computação para construir o futuro através da inovação, colaboração e excelência.
            </Typography>
            <SocialIcons />
          </motion.div>

          {/* Quick Links */}
          <FooterSection
            title="Links Rápidos"
            links={quickLinks}
          />

          {/* Resources */}
          <FooterSection
            title="Recursos"
            links={resources}
          />

          {/* Contact Info */}
          <FooterSection title="Informações de Contato">
            <div className="space-y-4">
              <motion.div
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ duration: 0.4, delay: 0.3 }}
              >
                <a
                  href="mailto:contato.ccfei@gmail.com"
                  className="block hover:text-white transition-colors duration-200"
                >
                  contato.ccfei@gmail.com
                </a>
              </motion.div>

              <motion.div
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ duration: 0.4, delay: 0.5 }}
                className="text-gray-300"
              >
                Salinha prédio I
              </motion.div>
            </div>
          </FooterSection>
        </div>

        {/* Copyright Section */}
        <motion.div
          className="border-t border-gray-600 mt-12 pt-8"
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ duration: 0.6, delay: 0.8 }}
        >
          <Typography variant="body" className="text-center text-gray-400" align="center">
            © {new Date().getFullYear()} Coruja Overflow. Todos os direitos reservados.
          </Typography>
        </motion.div>
      </div>
    </footer>
  );
}; 