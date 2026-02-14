"use client";

import {
  AdminListManager,
  UserFilter,
  UserFilterOptions,
  ManageUserCard,
} from "@/components";
import { apiService } from "@/services/api";
import { User } from "@/types";
import { useState, useCallback, useEffect } from "react";
import { toast } from "sonner";

export default function AdminUsuariosPage() {
  const [filters, setFilters] = useState<UserFilterOptions>({});
  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    totalPages: 1,
    totalItems: 0,
    itemsPerPage: 9
  });

  const fetchUsers = useCallback(async () => {
    setIsLoading(true);
    try {
      const response = await apiService.searchUsers({
        searchQuery: filters.searchQuery,
        createdFrom: filters.createdFrom,
        createdTo: filters.createdTo,
        role: filters.role,
        course: filters.course,
        isActive: filters.isActive,
        page: pagination.currentPage,
        limit: pagination.itemsPerPage
      });

      setUsers(response.users);
      setPagination(prev => ({
        ...prev,
        totalItems: response.totalCount,
        totalPages: Math.ceil(response.totalCount / prev.itemsPerPage)
      }));
    } catch (error) {
      toast.error("Erro ao carregar usuários");
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  }, [filters, pagination.currentPage, pagination.itemsPerPage]);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  const handleFilterChange = useCallback((newFilters: UserFilterOptions) => {
    setFilters(newFilters);
    setPagination(prev => ({ ...prev, currentPage: 1 })); // Reset to first page
  }, []);

  const handlePageChange = useCallback((page: number) => {
    setPagination(prev => ({ ...prev, currentPage: page }));
  }, []);

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* 1. Header */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">
            Gerenciar Usuários
          </h1>
          <p className="text-gray-500 text-sm">
            Adicione, edite ou remova usuários da plataforma.
          </p>
        </div>
      </div>

      {/* 2. LISTA DE USUÁRIOS INTEGRADA COM FILTROS */}
      <AdminListManager
        isLoading={isLoading}
        totalItems={pagination.totalItems}
        currentPage={pagination.currentPage}
        totalPages={pagination.totalPages}
        onPageChange={handlePageChange}
        resourceName="usuários"
        emptyMessage="Nenhum usuário encontrado com os filtros selecionados."
        filters={
          <UserFilter
            onFilterChange={handleFilterChange}
            roles={[
              { label: "Administrador", value: "admin" },
              { label: "Membro", value: "member" },
              { label: "Diretoria", value: "directorate" },
            ]}
          />
        }
      >
        {users.map((user) => (
          <ManageUserCard
            key={user.id}
            user={user}
          />
        ))}
      </AdminListManager>
    </div>
  );
}
