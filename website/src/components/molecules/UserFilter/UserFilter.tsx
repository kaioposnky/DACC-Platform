"use client";

import { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { Input, Select } from "@/components";
import { useDebounce } from "@/hooks/useDebounce";

export interface UserFilterOptions {
    searchQuery?: string;
    createdFrom?: string;
    createdTo?: string;
    role?: string;
    course?: string;
    isActive?: boolean | null;
}

interface UserFilterProps {
    onFilterChange: (filters: UserFilterOptions) => void;
    roles?: Array<{ label: string; value: string }>;
    courses?: Array<{ label: string; value: string }>;
    className?: string; // Standardize prop
}

export const UserFilter = ({
    onFilterChange,
    roles = [],
    courses = [],
    className = "",
}: UserFilterProps) => {
    const [searchQuery, setSearchQuery] = useState("");
    const [createdFrom, setCreatedFrom] = useState("");
    const [createdTo, setCreatedTo] = useState("");
    const [role, setRole] = useState("");
    const [course, setCourse] = useState("");
    const [isActive, setIsActive] = useState<string>("");

    // Debounce search
    const debouncedSearch = useDebounce(searchQuery, 600);
    const debouncedCreatedFrom = useDebounce(createdFrom, 600);
    const debouncedCreatedTo = useDebounce(createdTo, 600);

    // Effect to trigger filter change automatically
    useEffect(() => {
        const filters: UserFilterOptions = {
            searchQuery: debouncedSearch || undefined,
            createdFrom: debouncedCreatedFrom ? `${debouncedCreatedFrom}T00:00:00Z` : undefined,
            createdTo: debouncedCreatedTo ? `${debouncedCreatedTo}T23:59:59Z` : undefined,
            role: role || undefined,
            course: course || undefined,
            isActive: isActive === "" ? null : isActive === "true",
        };
        onFilterChange(filters);
    }, [debouncedSearch, debouncedCreatedFrom, debouncedCreatedTo, role, course, isActive, onFilterChange]);

    return (
        <motion.div
            className={`bg-gray-50 border-b border-gray-200 py-8 ${className}`}
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6 }}
        >
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex flex-col gap-6">
                    {/* Top Row: Primary Filters & Search */}
                    <div className="flex flex-col lg:flex-row lg:items-end lg:justify-between gap-6">

                        {/* Search Input */}
                        <div className="w-full lg:w-auto lg:min-w-87.5">
                            <Input
                                label="Pesquisar"
                                type="text"
                                placeholder="Nome, email ou matrícula..."
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)}
                                className="h-11"
                            />
                        </div>

                        {/* Filters Group 1: Role, Course, Status */}
                        <div className="flex flex-col sm:flex-row gap-4 flex-1 flex-wrap">
                            <div className="min-w-45 w-full sm:w-auto">
                                <Select
                                    label="Cargo"
                                    value={role}
                                    onChange={(e) => setRole(e.target.value)}
                                    options={[
                                        { label: "Todos os cargos", value: "" },
                                        ...roles,
                                    ]}
                                    className="h-11"
                                />
                            </div>

                            <div className="min-w-37.5 w-full sm:w-auto">
                                <Select
                                    label="Status"
                                    value={isActive}
                                    onChange={(e) => setIsActive(e.target.value)}
                                    options={[
                                        { label: "Todos", value: "" },
                                        { label: "Ativos", value: "true" },
                                        { label: "Inativos", value: "false" },
                                    ]}
                                    className="h-11"
                                />
                            </div>
                        </div>
                    </div>

                    {/* Bottom Row: Date Filters (Optional/Secondary) */}
                    <div className="flex flex-col sm:flex-row gap-4 border-t border-gray-200 pt-4">
                        <div className="w-full sm:w-auto">
                            <Input
                                type="date"
                                label="Cadastrado a partir de"
                                value={createdFrom}
                                onChange={(e) => setCreatedFrom(e.target.value)}
                                className="h-11"
                            />
                        </div>
                        <div className="w-full sm:w-auto">
                            <Input
                                type="date"
                                label="Cadastrado até"
                                value={createdTo}
                                onChange={(e) => setCreatedTo(e.target.value)}
                                className="h-11"
                            />
                        </div>
                    </div>
                </div>
            </div>
        </motion.div>
    );
};
