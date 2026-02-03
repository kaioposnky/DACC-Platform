"use client";

import {
  Squares2X2Icon,
  UsersIcon,
  ArrowLeftOnRectangleIcon,
  StarIcon,
  NewspaperIcon,
  BuildingLibraryIcon,
  ClipboardDocumentListIcon,
  ShieldCheckIcon,
  TagIcon,
  HomeIcon
} from "@heroicons/react/24/solid";
import Link from "next/link";
import { useAuth } from "@/context/AuthContext";
import { usePathname, useRouter } from "next/navigation";
import Image from "next/image";

const menuItems = [
  { name: "Home", href: "/", icon: HomeIcon },
  { name: "Dashboard", href: "/admin", icon: Squares2X2Icon },
  { name: "Avaliacoes", href: "/admin/avaliacoes", icon: StarIcon },
  { name: "Conteúdo", href: "/admin/conteudo", icon: NewspaperIcon },
  { name: "Faculdade", href: "/admin/faculdade", icon: BuildingLibraryIcon },
  { name: "Pedidos", href: "/admin/pedidos", icon: ClipboardDocumentListIcon },
  { name: "Permissões", href: "/admin/permissoes", icon: ShieldCheckIcon },
  { name: "Produtos", href: "/admin/produtos", icon: TagIcon },
  { name: "Usuários", href: "/admin/usuarios", icon: UsersIcon },
];

export const NavigationAdmin = () => {
  const { user, logout } = useAuth();
  const pathname = usePathname();
  const router = useRouter();

  const handleLogout = () => {
    logout();
    router.push("/");
  };

  return (
    <aside className="fixed left-0 top-0 h-screen w-64 bg-[#0a0b10] text-white flex flex-col z-50 border-r border-zinc-800/50">
      {/* Logo Header */}
      <div className="p-6 flex items-center gap-3">
        <div className="p-2 rounded-xl shadow-[0_0_15px_rgba(37,99,235,0.4)]">
          <Image src='https://i.postimg.cc/WzRPmW3r/LOGO-DACC-OFICIAL.png' alt="Logo DACC" width={24} height={24}></Image>
        </div>
        <div>
          <h2 className="text-xl font-bold tracking-tight">Coruja Overflow</h2>
          <p className="text-[10px] text-zinc-500 font-bold uppercase tracking-widest leading-none">
            Administração
          </p>
        </div>
      </div>

      {/* Menu Principal Section */}
      <div className="flex-1 px-4 py-4 space-y-8">
        <div>
          <h3 className="px-4 text-[11px] font-bold text-zinc-500 uppercase tracking-widest mb-4">
            Dashboard
          </h3>
          <nav className="space-y-2">
            {menuItems.map((item) => {
              const isActive = pathname === item.href;
              const Icon = item.icon;

              return (
                <Link
                  key={item.href}
                  href={item.href}
                  className={`flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 group ${isActive
                    ? "bg-blue-600/10 text-blue-500 font-semibold"
                    : "text-zinc-400 hover:bg-zinc-800/50 hover:text-zinc-200"
                    }`}
                >
                  <Icon className={`w-6 h-6 ${isActive ? "text-blue-500" : "text-zinc-400 group-hover:text-zinc-200"}`} />
                  <span className="text-sm">{item.name}</span>
                </Link>
              );
            })}
          </nav>
        </div>
      </div>

      {/* Footer / User Profile */}
      <div className="p-4 border-t border-zinc-800/50">
        <div className="flex items-center gap-3 px-2 mb-4">
          <div className="w-10 h-10 rounded-full bg-zinc-800 flex items-center justify-center font-bold text-blue-500 border border-zinc-700">
            {user?.name?.charAt(0) || "A"}
          </div>
          <div className="flex-1 min-w-0">
            <p className="text-sm font-semibold truncate">{user?.name || "Administrador"}</p>
            <p className="text-xs text-zinc-500 truncate">{user?.email || "admin@dacc.com"}</p>
          </div>
        </div>
        <button
          onClick={handleLogout}
          className="flex items-center gap-3 w-full px-4 py-2 text-zinc-400 hover:text-red-400 hover:bg-red-400/10 rounded-lg transition-all text-sm"
        >
          <ArrowLeftOnRectangleIcon className="w-5 h-5" />
          Sair do Painel
        </button>
      </div>
    </aside>
  );
};
