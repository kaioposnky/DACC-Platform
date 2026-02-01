export interface DashboardStats {
    users: {
        total: number;
        active: number;
        subscribers: number;
        newThisMonth: number;
        byRole: Record<string, number>;
    };
    orders: {
        total: number;
        totalRevenue: number;
        pending: number;
        salesLast30Days: number;
        byStatus: Record<string, number>;
    };
    products: {
        totalActive: number;
        lowStockCount: number;
        byCategory: Record<string, number>;
    };
    reviews: {
        total: number;
        averageRating: number;
        ratingDistribution: Record<string, number>;
    };
    events: {
        total: number;
        upcoming: number;
        byType: Record<string, number>;
    };
    news: {
        total: number;
        byCategory: Record<string, number>;
    };
    ads: {
        totalActive: number;
        byType: Record<string, number>;
    };
    faculty: {
        total: number;
        byTitle: Record<string, number>;
    };
    permissions: {
        totalDefinitions: number;
    };
}
