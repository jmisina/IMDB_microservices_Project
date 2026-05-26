import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getOrders, updateOrderStatus } from "./ordersApi";
import { Package, CheckCircle, CreditCard, Clock } from "lucide-react";

export const ManageOrders = () => {
  const queryClient = useQueryClient();
  const { data: orders, isLoading, isError } = useQuery({
    queryKey: ["orders"],
    queryFn: getOrders,
  });

  const updateMutation = useMutation({
    mutationFn: (id: number) => updateOrderStatus(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["orders"] });
      alert("Status zamówienia został zaktualizowany.");
    },
    onError: (err: any) => {
      alert("Błąd podczas aktualizacji zamówienia: " + (err.response?.data?.Message || err.message));
    }
  });

  if (isLoading) return <div className="text-black">Ładowanie zamówień...</div>;
  if (isError) return <div className="text-red-500">Błąd podczas ładowania zamówień.</div>;

  const getStatusBadge = (status: string) => {
    const s = status?.toUpperCase();
    if (s === 'COMPLETED') return <span className="bg-green-100 text-green-700 px-2 py-1 rounded-full text-xs font-bold">ZREALIZOWANE</span>;
    if (s === 'PENDING') return <span className="bg-yellow-100 text-yellow-700 px-2 py-1 rounded-full text-xs font-bold">OCZEKUJĄCE</span>;
    return <span className="bg-gray-100 text-gray-700 px-2 py-1 rounded-full text-xs font-bold">{status}</span>;
  };

  const getPaymentBadge = (status: string) => {
    const s = status?.toUpperCase();
    if (s === 'COMPLETED') return <span className="text-green-600 flex items-center gap-1"><CreditCard size={14} /> Opłacone</span>;
    return <span className="text-amber-600 flex items-center gap-1"><Clock size={14} /> Oczekiwanie</span>;
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow text-black">
      <h2 className="text-2xl font-bold mb-6 flex items-center gap-2">
        <Package className="text-blue-600" /> Zarządzanie Zamówieniami
      </h2>
      <div className="overflow-x-auto">
        <table className="w-full text-left">
          <thead>
            <tr className="border-b text-gray-600 text-sm">
              <th className="py-3">ID</th>
              <th>Data</th>
              <th>Suma</th>
              <th>Status Zamówienia</th>
              <th>Status Płatności</th>
              <th>Akcje</th>
            </tr>
          </thead>
          <tbody>
            {orders?.map((o: any) => (
              <tr key={o.id} className="border-b hover:bg-gray-50 transition-colors">
                <td className="py-4 font-medium text-blue-600">#{o.id}</td>
                <td className="text-sm">{new Date(o.orderDate).toLocaleString()}</td>
                <td className="font-bold">{o.paymentData?.amount || 0}zł</td>
                <td>{getStatusBadge(o.status)}</td>
                <td className="text-sm">{getPaymentBadge(o.paymentData?.status)}</td>
                <td>
                  {o.status?.toUpperCase() !== 'COMPLETED' && (
                    <button
                      onClick={() => {
                        if (confirm(`Czy chcesz oznaczyć zamówienie #${o.id} jako zrealizowane?`)) {
                          updateMutation.mutate(o.id);
                        }
                      }}
                      className="flex items-center gap-1 bg-green-50 text-green-600 px-3 py-1.5 rounded-lg hover:bg-green-100 transition-colors text-sm font-semibold border border-green-200"
                      disabled={updateMutation.isPending}
                    >
                      <CheckCircle size={16} />
                      Zrealizuj
                    </button>
                  )}
                </td>
              </tr>
            ))}
            {(!orders || orders.length === 0) && (
              <tr>
                <td colSpan={6} className="py-10 text-center text-gray-500">Brak zamówień w systemie.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};
