'use client';

import { useState } from 'react';
import { Footer, Navigation } from "@/components";
import { ProfileBanner, ProfileUser } from "@/components/organisms/ProfileBanner";
import { ProfileSettingsSidebar, SettingsSection } from "@/components/organisms/ProfileSettingsSidebar";
import { ProfileSettingsForm, UserFormData } from "@/components/organisms/ProfileSettingsForm";

export default function ProfilePage() {

    const [activeSection, setActiveSection] = useState<SettingsSection>('account');
  
  // Mock initial user data - in a real app, this would come from authentication/API
  const initialUserData: Partial<UserFormData> = {
    firstName: 'João',
    lastName: 'Silva',
    email: 'joao.silva@student.university.edu',
    phone: '+55 (11) 99999-9999',
    studentId: 'CS2022001',
    course: 'Ciência da Computação',
    academicYear: '2º Ano',
    expectedGraduation: '2026-05'
  };

  const handleSectionChange = (section: SettingsSection) => {
    setActiveSection(section);
  };

  const handleSaveForm = (data: UserFormData) => {
    console.log('Saving user data:', data);
    // Here you would typically send the data to your API
    alert('Dados salvos com sucesso!');
  };

  const handleResetForm = () => {
    console.log('Resetting form to initial values');
  };

  const renderContent = () => {
    switch (activeSection) {
      case 'account':
        return (
          <ProfileSettingsForm
            initialData={initialUserData}
            onSave={handleSaveForm}
            onReset={handleResetForm}
          />
        );
      case 'security':
        return (
          <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">Segurança</h2>
            <p className="text-gray-600">Configurações de segurança em desenvolvimento...</p>
          </div>
        );
      case 'preferences':
        return (
          <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">Preferências</h2>
            <p className="text-gray-600">Configurações de preferências em desenvolvimento...</p>
          </div>
        );
      case 'notifications':
        return (
          <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">Notificações</h2>
            <p className="text-gray-600">Configurações de notificações em desenvolvimento...</p>
          </div>
        );
      default:
        return null;
    }
  };
  // Mock user data - in a real app, this would come from authentication/API
  const [user] = useState<ProfileUser>({
    id: "1",
    name: "João Silva",
    email: "joao.silva@student.university.edu",
    role: "Estudante de Ciência da Computação",
    avatar: "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=500&q=80",
    stats: {
      orders: 15,
      reviews: 23,
      yearsActive: 2
    }
  });


  const handleChangeAvatar = () => {
    console.log('Alterar avatar clicado');
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      
      <ProfileBanner 
        user={user}
        onChangeAvatar={handleChangeAvatar}
      />

<div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="grid grid-cols-1 lg:grid-cols-4 gap-8">
          {/* Sidebar */}
          <div className="lg:col-span-1">
            <ProfileSettingsSidebar
              activeSection={activeSection}
              onSectionChange={handleSectionChange}
            />
          </div>

          {/* Main Content */}
          <div className="lg:col-span-3">
            {renderContent()}
          </div>
        </div>
      </div>
      
      <Footer />
    </div>
  );
}