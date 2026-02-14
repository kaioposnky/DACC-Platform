"use client";

import { useEffect, useState } from "react";
import { apiService } from "@/services/api";
import { News, Event, Project, Announcement } from "@/types";
import {
    AdminListManager,
    ManageNewsCard,
    ManageEventCard,
    ManageProjectCard,
    ConfirmationModal,
    Button,
} from "@/components";
import { toast } from "sonner";
import { PlusIcon } from "@heroicons/react/24/outline";
import { useRouter } from "next/navigation";
import { ManageAnnouncementCard } from "@/components/molecules/admin/ManageAnnouncementCard";

type ContentTab = "news" | "events" | "projects" | "announcements";

export default function AdminConteudosPage() {
    const router = useRouter();
    const [activeTab, setActiveTab] = useState<ContentTab>("news");
    const [news, setNews] = useState<News[]>([]);
    const [events, setEvents] = useState<Event[]>([]);
    const [projects, setProjects] = useState<Project[]>([]);
    const [announcements, setAnnouncements] = useState<Announcement[]>([]);

    const [isLoading, setIsLoading] = useState(true);
    const [totalItems, setTotalItems] = useState(0);
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 12;

    // Deletion state
    const [itemToDelete, setItemToDelete] = useState<{ id: string; title: string, type: ContentTab } | null>(null);
    const [isDeleting, setIsDeleting] = useState(false);

    const fetchData = async (page: number) => {
        setIsLoading(true);
        try {
            if (activeTab === "news") {
                const response = await apiService.getNews({ page, limit: itemsPerPage });
                setNews(response.news);
                setTotalItems(response.totalCount);
            } else if (activeTab === "events") {
                const response = await apiService.getEvents();
                setEvents(response);
                setTotalItems(response.length);
            } else if (activeTab === "projects") {
                const response = await apiService.getProjects({ page, limit: itemsPerPage });
                setProjects(response.projects);
                setTotalItems(response.totalCount);
            } else if (activeTab === "announcements") {
                const response = await apiService.getAnnouncements({ page, limit: itemsPerPage });
                setAnnouncements(response.announcements);
                setTotalItems(response.totalCount);
            }
        } catch (error) {
            console.error(`Erro ao buscar ${activeTab}:`, error);
            toast.error(`Erro ao carregar ${activeTab}`);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        setCurrentPage(1);
        fetchData(1);
    }, [activeTab]);

    const handlePageChange = (page: number) => {
        setCurrentPage(page);
        fetchData(page);
    };

    const handleDelete = async () => {
        if (!itemToDelete) return;

        setIsDeleting(true);
        try {
            if (itemToDelete.type === "news") await apiService.deleteNews(itemToDelete.id);
            else if (itemToDelete.type === "events") await apiService.deleteEvent(itemToDelete.id);
            else if (itemToDelete.type === "projects") await apiService.deleteProject(itemToDelete.id);

            toast.success("Item removido com sucesso!");
            fetchData(currentPage);
        } catch (error) {
            console.error("Erro ao deletar:", error);
            toast.error("Erro ao remover item");
        } finally {
            setIsDeleting(false);
            setItemToDelete(null);
        }
    };

    const tabs = [
        { id: "news", label: "Notícias", icon: "📰" },
        { id: "events", label: "Eventos", icon: "📅" },
        { id: "projects", label: "Projetos", icon: "🚀" },
        { id: "announcements", label: "Anúncios", icon: "📢" },
    ];

    const handleCreateNew = () => {
        const routeMap = {
            news: "/admin/conteudo/noticias/new",
            events: "/admin/conteudo/eventos/new",
            projects: "/admin/conteudo/projetos/new",
            announcements: "/admin/conteudo/anuncios/new",
        };
        router.push(routeMap[activeTab]);
    };

    return (
        <div className="p-6 max-w-7xl mx-auto space-y-6">
            {/* Header */}
            <div className="flex flex-col md:flex-row md:justify-between md:items-end gap-4">
                <div>
                    <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Conteúdos</h1>
                    <p className="text-gray-500 mt-1">Gerencie as notícias, eventos e projetos da plataforma.</p>
                </div>
                <Button
                    onClick={handleCreateNew}
                    className="flex items-center gap-2"
                >
                    <PlusIcon className="w-5 h-5" />
                    Novo {tabs.find(t => t.id === activeTab)?.label.slice(0, -1)}
                </Button>
            </div>

            {/* Tabs */}
            <div className="flex bg-gray-100 p-1 rounded-xl w-fit">
                {tabs.map((tab) => (
                    <button
                        key={tab.id}
                        onClick={() => setActiveTab(tab.id as ContentTab)}
                        className={`flex items-center gap-2 px-6 py-2.5 rounded-lg text-sm font-bold transition-all ${activeTab === tab.id
                            ? "bg-white text-primary shadow-sm"
                            : "text-gray-500 hover:text-gray-700 hover:bg-white/50"
                            }`}
                    >
                        <span>{tab.icon}</span>
                        {tab.label}
                    </button>
                ))}
            </div>

            {/* Content List */}
            <AdminListManager
                isLoading={isLoading}
                totalItems={totalItems}
                currentPage={currentPage}
                totalPages={Math.ceil(totalItems / itemsPerPage)}
                onPageChange={handlePageChange}
                resourceName={tabs.find(t => t.id === activeTab)?.label.toLowerCase() || "itens"}
                gridClassName="grid grid-cols-1 md:grid-cols-2 gap-4"
            >
                {activeTab === "news" && news.map(item => (
                    <ManageNewsCard
                        key={item.id}
                        news={item}
                        onDelete={(n) => setItemToDelete({ id: n.id, title: n.title, type: "news" })}
                    />
                ))}
                {activeTab === "events" && events.map(item => (
                    <ManageEventCard
                        key={item.id}
                        event={item}
                        onDelete={(e) => setItemToDelete({ id: e.id, title: e.title, type: "events" })}
                    />
                ))}
                {activeTab === "projects" && projects.map(item => (
                    <ManageProjectCard
                        key={item.id}
                        project={item}
                        onDelete={(p) => setItemToDelete({ id: p.id, title: p.title, type: "projects" })}
                    />
                ))}
                {activeTab === "announcements" && announcements.map(item => (
                    <ManageAnnouncementCard
                        key={item.id}
                        announcement={item}
                        onDelete={(a) => setItemToDelete({ id: a.id, title: a.title, type: "announcements" })}
                    />
                ))}
            </AdminListManager>

            {/* Delete Modal */}
            <ConfirmationModal
                isOpen={!!itemToDelete}
                onClose={() => setItemToDelete(null)}
                onConfirm={handleDelete}
                isLoading={isDeleting}
                title={`Excluir ${tabs.find(t => t.id === itemToDelete?.type)?.label.slice(0, -1)}`}
                message={`Tem certeza que deseja excluir "${itemToDelete?.title}"? Esta ação é irreversível.`}
                confirmLabel="Sim, Excluir"
                variant="danger"
            />
        </div>
    );
}
