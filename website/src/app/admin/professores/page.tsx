"use client";

import {
  AdminListManager,
  SearchBar,
  UserFilter,
  UserFilterOptions,
  Button,
} from "@/components";
import { ManageFacultyCard } from "@/components/molecules/admin/ManageFacultyCard";
import { useDebounce } from "@/hooks/useDebounce";
import { apiService } from "@/services/api";
import { Faculty } from "@/types";
import { useState, useCallback, useEffect } from "react";
import { toast } from "sonner";
import { PlusIcon } from "@heroicons/react/24/outline";
import Link from "next/link";

export default function AdminProfessoresPage() {
  const [query, setQuery] = useState('');
  const [faculty, setFaculty] = useState<Faculty[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    totalPages: 1,
    totalItems: 0,
    itemsPerPage: 9
  });

  const debouncedSearch = useDebounce(query, 600);

  const fetchFaculty = useCallback(async () => {
    setIsLoading(true);
    try {
      const response = await apiService.getFaculty({
        search: debouncedSearch,
        page: pagination.currentPage,
        limit: pagination.itemsPerPage
      });

      setFaculty(response.faculty);
      setPagination(prev => ({
        ...prev,
        totalItems: response.totalCount,
        totalPages: Math.ceil(response.totalCount / prev.itemsPerPage)
      }));
    } catch (error) {
      toast.error("Erro ao carregar professores");
    } finally {
      setIsLoading(false);
    }
  }, [query, pagination.currentPage, pagination.itemsPerPage]);

  useEffect(() => {
    fetchFaculty();
  }, [fetchFaculty]);

  const handleDelete = async (faculty: Faculty) => {
    try {
      await apiService.deleteFacultyMember(faculty.id);
      toast.success("Professor removido com sucesso");
      fetchFaculty();
    } catch (error) {
      toast.error("Erro ao remover professor");
      console.error(error);
    }
  };

  const handleQueryChange = useCallback((query: string) => {
    setQuery(query);
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
            Gerenciar Professores
          </h1>
          <p className="text-gray-500 text-sm">
            Adicione, edite ou remova professores da plataforma.
          </p>
        </div>
        <Link href="/admin/professores/new">
          <Button variant="primary" className="flex items-center gap-2">
            <PlusIcon className="w-5 h-5" />
            Novo Professor
          </Button>
        </Link>
      </div>

      {/* 2. LISTA DE PROFESSORES INTEGRADA COM FILTROS */}
      <AdminListManager
        isLoading={isLoading}
        totalItems={pagination.totalItems}
        currentPage={pagination.currentPage}
        totalPages={pagination.totalPages}
        onPageChange={handlePageChange}
        resourceName="professores"
        emptyMessage="Nenhum usuário encontrado com os filtros selecionados."
        filters={
          <SearchBar
            onSearch={handleQueryChange}
            placeholder="Pesquisar professores..."
          />
        }
      >
        {faculty.map((faculty) => (
          <ManageFacultyCard
            key={faculty.id}
            faculty={faculty}
            onDelete={handleDelete}
          />
        ))}
      </AdminListManager>
    </div>
  );
}
