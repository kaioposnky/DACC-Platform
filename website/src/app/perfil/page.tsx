'use client';

import { useState, useEffect } from 'react';
import { useAuth } from '@/context/AuthContext';
import { Footer, Navigation } from "@/components";
import { ProfileBanner, ProfileUser } from "@/components/organisms/ProfileBanner";
import { ProfileSettingsSidebar, SettingsSection } from "@/components/organisms/ProfileSettingsSidebar";
import { ProfileSettingsForm, UserFormData } from "@/components/organisms/ProfileSettingsForm";
import {apiService} from "@/services/api";
import {UserStats} from "@/types";

export default function ProfilePage() {
  const { user, isLoading } = useAuth();
  const [activeSection, setActiveSection] = useState<SettingsSection>('account');
  const [userStats, setUserStats] = useState<UserStats>({
    orders: 1,
    reviews: 1,
    registryDate: "22/12/2025",
  });
  
  useEffect(() => {
    const fetchStats = async () => {
      if (user?.id) {
        try {
          const stats = await apiService.getUserStats(user.id);
          setUserStats(stats);
        } catch (error) {
          console.error('Erro ao buscar estatísticas do usuário:', error);
        }
      }
    };
    if (user) fetchStats();
  }, [user]);

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

  const handleChangeAvatar = () => {
    console.log('Alterar avatar clicado');
  };

  // User loading, default template if user data loading
  if (isLoading || !user) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <Navigation />
        <div className="text-center">
          <p className="text-lg font-semibold">Carregando perfil...</p>
        </div>
        <Footer />
      </div>
    );
  }
  
  const profileUser: ProfileUser = {
    id: user.id,
    name: `${user.name} ${user.lastName || ''}`.trim(),
    email: user.email,
    role: user.role,
    avatar: user.avatar || "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=500&q=80",
    stats: userStats
  };

  const formUserData: Partial<UserFormData> = {
    firstName: user.name,
    lastName: user.lastName,
    email: user.email,
    phone: user.phone,
    studentId: user.ra,
    course: user.course,
  };

  const renderContent = () => {
    switch (activeSection) {
      case 'account':
        return (
          <ProfileSettingsForm
            initialData={formUserData}
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

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      
      <ProfileBanner 
        user={profileUser}
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
