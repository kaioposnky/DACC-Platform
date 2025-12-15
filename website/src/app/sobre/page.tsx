import { FeaturesSection, Footer, Navigation, ProjectsSection, StatsSection, FacultySection } from "@/components/organisms";

import { StatData } from "@/types";

export default function Sobre() {
  const courseStats: StatData[] = [
    {
      id: '1',
      number: '4',
      label: 'ANOS',
      animateNumber: true
    },
    {
      id: '2',
      number: '12',
      label: 'DISCIPLINAS',
      animateNumber: true
    },
    {
      id: '3',
      number: '50',
      label: 'PROJETOS',
      suffix: '+',
      animateNumber: true
    },
    {
      id: '4',
      number: '95',
      label: 'TAXA DE SUCESSO',
      suffix: '%',
      animateNumber: true
    }
  ];

  return (
    <>
      <Navigation />
      <StatsSection
        title="Sobre o DACC"
        subtitle="Descubra o currículo abrangente que molda a próxima geração de inovadores em tecnologia"
        stats={courseStats}
        backgroundColor="primary"
        showDecorations={true}
        titleColor="white"
        subtitleColor="white"
      />

      <FeaturesSection />

      <ProjectsSection />

      <FacultySection
        title="Nosso Corpo Docente"
        subtitle="Conheça os profissionais experientes que orientam nossos estudantes"
        backgroundColor="gray"
      />
      <Footer />
    </>

  );
}