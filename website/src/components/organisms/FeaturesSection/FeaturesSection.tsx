"use client";

import { Typography } from '@/components/atoms';
import { FeatureCard } from '@/components/molecules';
import { CodeBracketIcon, RocketLaunchIcon, UsersIcon } from '@heroicons/react/24/outline';

interface FeaturesSectionProps {
  className?: string;
}

const features = [
  {
    id: '1',
    title: 'Apoie o DACC',
    description: 'Faça parte da história da Ciência da Computação da FEI. Venha somar e criar oportunidades incríveis para nossa comunidade.',
    icon: <CodeBracketIcon className="w-10 h-10" />,
  },
  {
    id: '2',
    title: 'Projetos Inovadores',
    description: 'Desenvolva projetos reais e impactantes com mentoria de professores e profissionais da indústria.',
    icon: <RocketLaunchIcon className="w-10 h-10" />,
  },
  {
    id: '3',
    title: 'Networking',
    description: 'Conecte-se com estudantes, alumni e profissionais para expandir sua rede e descobrir oportunidades.',
    icon: <UsersIcon className="w-10 h-10" />,
  },
];

export const FeaturesSection = ({ className = '' }: FeaturesSectionProps) => {
  return (
    <section className={`py-16 bg-white ${className}`}>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Section Header */}
        <div className="text-center mb-16">
          <Typography variant="h2" color="primary" className="mb-4 font-bold" align="center">
            Por que escolher a Ciência da Computação?
          </Typography>
          <Typography variant="subtitle" color="gray" className="max-w-2xl mx-auto" align="center">
            Descubra as oportunidades incríveis que nossa comunidade oferece para impulsionar sua carreira em tecnologia.
          </Typography>
        </div>

        {/* Features Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 lg:gap-12">
          {features.map((feature) => (
            <FeatureCard
              key={feature.id}
              icon={feature.icon}
              title={feature.title}
              description={feature.description}
              className="h-full"
            />
          ))}
        </div>
      </div>
    </section>
  );
}; 