import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { updateUserRole } from "../user/userApi";
import apiClient from "../../api/apiClient";
import { Users, Trash2, Shield, User as UserIcon, Briefcase, AlertTriangle, X, Check } from "lucide-react";
import { useAuthStore } from "../../store/useAuthStore";
import { useState } from "react";

export const ManageUsers = () => {
  const queryClient = useQueryClient();
  const currentUser = useAuthStore(state => state.user);
  const [selectedUser, setSelectedUser] = useState<any>(null);
  const [newRole, setNewRole] = useState<string>("");

  const { data: users, isLoading, isError } = useQuery({
    queryKey: ["users"],
    queryFn: async () => {
      const response = await apiClient.get('/user-service/users');
      return response.data;
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => apiClient.delete(`/user-service/users/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["users"] }),
  });

  const roleMutation = useMutation({
    mutationFn: ({ id, role }: { id: number; role: string }) => updateUserRole(id, role),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["users"] });
      setSelectedUser(null);
    },
    onError: (err: any) => {
      alert("Błąd podczas zmiany roli: " + (err.response?.data?.Message || err.message));
    }
  });

  if (isLoading) return <div className="text-black">Ładowanie użytkowników...</div>;
  if (isError) return <div className="text-red-500">Błąd podczas ładowania użytkowników.</div>;

  const roleConfig: any = {
    ADMIN: { color: "bg-purple-100 text-purple-700", icon: Shield },
    MANAGER: { color: "bg-blue-100 text-blue-700", icon: Briefcase },
    USER: { color: "bg-gray-100 text-gray-700", icon: UserIcon },
  };

  const openRoleModal = (user: any) => {
    setSelectedUser(user);
    setNewRole(user.role);
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow text-black">
      <h2 className="text-2xl font-bold mb-6 flex items-center gap-2">
        <Users className="text-purple-600" /> Zarządzanie Użytkownikami
      </h2>
      <div className="overflow-x-auto">
        <table className="w-full text-left">
          <thead>
            <tr className="border-b text-gray-600">
              <th className="py-2">Użytkownik</th>
              <th>Email</th>
              <th>Aktualna Rola</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody>
            {users?.map((u: any) => {
              const RoleIcon = roleConfig[u.role]?.icon || UserIcon;
              return (
                <tr key={u.id} className="border-b hover:bg-gray-50 transition-colors">
                  <td className="py-4">
                    <div className="font-medium">{u.username}</div>
                    <div className="text-xs text-gray-400">{u.firstName} {u.lastName}</div>
                  </td>
                  <td>{u.email}</td>
                  <td>
                    <span className={`inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-bold ${roleConfig[u.role]?.color}`}>
                      <RoleIcon size={12} />
                      {u.role}
                    </span>
                  </td>
                  <td>
                    <div className="flex items-center gap-4">
                      <button
                        onClick={() => openRoleModal(u)}
                        disabled={u.id === currentUser?.id}
                        className="text-blue-600 hover:text-blue-800 text-sm font-medium disabled:opacity-30 disabled:cursor-not-allowed"
                      >
                        Zmień rolę
                      </button>
                      <button
                        onClick={() => {
                          if (confirm(`Czy na pewno chcesz usunąć użytkownika ${u.username}?`)) {
                            deleteMutation.mutate(u.id);
                          }
                        }}
                        className="text-red-500 hover:text-red-700 disabled:opacity-30"
                        disabled={deleteMutation.isPending || u.id === currentUser?.id}
                      >
                        <Trash2 size={18} />
                      </button>
                    </div>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      {/* Role Management Modal */}
      {selectedUser && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50 animate-in fade-in duration-200">
          <div className="bg-white rounded-xl shadow-2xl max-w-md w-full overflow-hidden">
            <div className="p-6">
              <div className="flex justify-between items-center mb-6">
                <h3 className="text-xl font-bold">Zarządzaj Uprawnieniami</h3>
                <button onClick={() => setSelectedUser(null)} className="text-gray-400 hover:text-gray-600">
                  <X size={24} />
                </button>
              </div>

              <div className="mb-6">
                <p className="text-gray-600 text-sm mb-2">Użytkownik:</p>
                <div className="flex items-center gap-3 bg-gray-50 p-3 rounded-lg">
                  <div className="bg-blue-100 p-2 rounded-full text-blue-600">
                    <UserIcon size={20} />
                  </div>
                  <div>
                    <div className="font-bold text-gray-800">{selectedUser.username}</div>
                    <div className="text-xs text-gray-500">{selectedUser.email}</div>
                  </div>
                </div>
              </div>

              <div className="space-y-3">
                <p className="text-gray-600 text-sm mb-2">Wybierz nową rolę:</p>
                {Object.keys(roleConfig).map((role) => {
                  const Config = roleConfig[role];
                  const Icon = Config.icon;
                  const isSelected = newRole === role;
                  return (
                    <button
                      key={role}
                      onClick={() => setNewRole(role)}
                      className={`w-full flex items-center justify-between p-4 rounded-xl border-2 transition-all ${
                        isSelected 
                          ? "border-blue-500 bg-blue-50" 
                          : "border-gray-100 hover:border-gray-200"
                      }`}
                    >
                      <div className="flex items-center gap-3">
                        <div className={`p-2 rounded-lg ${Config.color}`}>
                          <Icon size={20} />
                        </div>
                        <span className={`font-bold ${isSelected ? "text-blue-700" : "text-gray-700"}`}>
                          {role}
                        </span>
                      </div>
                      {isSelected && <Check className="text-blue-500" size={24} />}
                    </button>
                  );
                })}
              </div>

              {newRole !== selectedUser.role && (
                <div className="mt-6 p-4 bg-amber-50 rounded-lg flex gap-3 border border-amber-100">
                  <AlertTriangle className="text-amber-500 shrink-0" size={20} />
                  <p className="text-xs text-amber-800">
                    Uwaga: Zmiana roli wpłynie na dostęp tego użytkownika do określonych części systemu.
                  </p>
                </div>
              )}
            </div>

            <div className="bg-gray-50 p-6 flex gap-3">
              <button
                onClick={() => setSelectedUser(null)}
                className="flex-1 px-4 py-2 bg-white border border-gray-300 rounded-lg font-bold text-gray-700 hover:bg-gray-100 transition-colors"
              >
                Anuluj
              </button>
              <button
                disabled={newRole === selectedUser.role || roleMutation.isPending}
                onClick={() => roleMutation.mutate({ id: selectedUser.id, role: newRole })}
                className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg font-bold hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-all"
              >
                {roleMutation.isPending ? "Zapisywanie..." : "Potwierdź zmianę"}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
