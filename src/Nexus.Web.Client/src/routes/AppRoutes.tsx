import {
    Navigate,
    Route,
    Routes,
} from 'react-router-dom'

import { AppLayout } from '../layouts/AppLayout'
import { CreateWorkspacePage } from '../pages/CreateWorkspacePage'
import { DashboardPage } from '../pages/DashboardPage'
import { IntelligencePage } from '../pages/IntelligencePage'
import { NotFoundPage } from '../pages/NotFoundPage'
import { ProjectDetailsPage } from '../pages/ProjectDetailsPage'
import { SettingsPage } from '../pages/SettingsPage'
import { WorkspaceSettingsPage } from '../pages/WorkspaceSettingsPage'
import { WorkspacesPage } from '../pages/WorkspacesPage'

export function AppRoutes() {
    return (
        <Routes>
            <Route
                path="/"
                element={
                    <Navigate
                        to="/dashboard"
                        replace
                    />
                }
            />

            <Route element={<AppLayout />}>
                <Route
                    path="/dashboard"
                    element={<DashboardPage />}
                />

                <Route
                    path="/workspaces"
                    element={<WorkspacesPage />}
                />

                <Route
                    path="/workspaces/new"
                    element={<CreateWorkspacePage />}
                />

                <Route
                    path="/workspaces/settings"
                    element={<WorkspaceSettingsPage />}
                />

                <Route
                    path="/projects/:projectId"
                    element={<ProjectDetailsPage />}
                />

                <Route
                    path="/intelligence"
                    element={<IntelligencePage />}
                />

                <Route
                    path="/settings"
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