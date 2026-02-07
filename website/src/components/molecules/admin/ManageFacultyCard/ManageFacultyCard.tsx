"use client";

import { Faculty } from "@/types";
import { AdminCard, Button } from "@/components";
import {
  UserCircleIcon,
  PencilIcon,
  TrashIcon,
} from "@heroicons/react/24/outline";
import Link from "next/link";
import { formatDate } from "@/utils/formatters";
import { getSocialIcon } from "../../SocialIcons";

interface ManageFacultyCardProps {
  faculty: Faculty;
  onDelete: (faculty: Faculty) => void;
}

export const ManageFacultyCard = ({ faculty, onDelete }: ManageFacultyCardProps) => {
  return (
    <AdminCard
      title=""
      className="h-full flex flex-col justify-between group hover:border-primary/50 transition-colors"
    >
      <div className="space-y-4">
        {/* Header: Avatar + Info Principal */}
        <div className="flex items-start justify-between gap-4">
          <div className="flex items-center gap-3">
            {faculty.imageUrl ? (
              <img
                src={faculty.imageUrl}
                alt={faculty.name}
                className="w-12 h-12 rounded-full object-cover border border-gray-100"
              />
            ) : (
              <div className="w-12 h-12 rounded-full bg-primary/10 flex items-center justify-center text-primary">
                <UserCircleIcon className="w-8 h-8" />
              </div>
            )}
            <div>
              <h3
                className="font-bold text-gray-900 line-clamp-1"
                title={faculty.title + " " + faculty.name}
              >
                {faculty.title + " " + faculty.name}
              </h3>
              <p
                className="text-sm text-gray-500 line-clamp-1"
                title={faculty.social.email}
              >
                {faculty.social.email}
              </p>
              {/* Ícones de redes sociais */}

              <div className="flex flex-row space-x-5">
                {faculty.social.github &&
                  <a
                    href={faculty.social.github}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    {getSocialIcon("github")}
                  </a>
                }
                {faculty.social.linkedin &&
                  <a
                    href={faculty.social.linkedin}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    {getSocialIcon("linkedin")}
                  </a>
                }
              </div>
            </div>
          </div>
        </div>
        {/* Detalhes: Cargo, Curso, Data */}
        <div className="space-y-2 text-sm text-gray-600">
          <div className="flex justify-between border-b border-gray-50 pb-2">
            <span>Cargo:</span>
            <span className="font-medium text-gray-900">
              {faculty.position || "N/A"}
            </span>
          </div>
          <div className="flex justify-between border-b border-gray-50 pb-2">
            <span>Especialização:</span>
            <span className="font-medium text-gray-900">
              {faculty.specialization || "N/A"}
            </span>
          </div>
          <div className="flex justify-between border-b border-gray-50 pb-2">
            <span>Cadastro:</span>
            <span className="font-medium text-gray-900">
              {faculty.createdAt ? formatDate(faculty.createdAt) : "N/A"}
            </span>
          </div>
          <div className="flex justify-between border-b border-gray-50 pb-2">
            <span>Última atualização:</span>
            <span className="font-medium text-gray-900">
              {faculty.updatedAt ? formatDate(faculty.updatedAt) : "N/A"}
            </span>
          </div>
        </div>
      </div>

      {/* Ações */}
      <div className="pt-4 mt-4 border-t border-gray-100 flex gap-2">
        <Link href={`/admin/professores/${faculty.id}`} className="flex-1">
          <Button variant="secondary" className="w-full text-sm">
            <PencilIcon className="w-4 h-4 mr-2" />
            Editar
          </Button>
        </Link>
        <Button variant="secondary" className="w-full text-sm p-2 bg-red-50 text-red-600 rounded-lg hover:bg-red-100 transition-colors" onClick={() => onDelete(faculty)}>
          <TrashIcon className="w-4 h-4 mr-2" />
          Excluir
        </Button>
      </div>
    </AdminCard>
  );
};
