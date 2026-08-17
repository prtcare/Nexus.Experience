import {
    Navigate,
    Route,
    Routes,
} from 'react-router-dom'

import { AppLayout } from '../layouts/AppLayout'
import { DashboardPage } from '../pages/DashboardPage'
import { ProductsPage } from '../pages/ProductsPage'
import { IntelligencePage } from '../pages/IntelligencePage'
import { SettingsPage } from '../pages/SettingsPage'
import { NotFoundPage } from '../pages/NotFoundPage'
import { WorkspacesPage } from '../pages/WorkspacesPage'

export function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<AppLayout />}>
                <Route
                    index
                    element={<Navigate to="/dashboard" replace />}
                />

                <Route
                    path="dashboard"
                    element={<DashboardPage />}
                />
                <Route
                    path="/workspaces"
                    element={<WorkspacesPage />}
                />
                <Route
                    path="products"
                    element={<ProductsPage />}
                />

                <Route
                    path="intelligence"
                    element={<IntelligencePage />}
                />

                <Route
                    path="settings"
                    element={<SettingsPage />}
                />
            </Route>

            <Route
                path="*"
                element={<NotFoundPage />}
            />
        </Routes>
    )
}