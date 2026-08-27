import {
    Navigate,
    Route,
    Routes,
} from 'react-router-dom'

import { AppLayout } from '../layouts/AppLayout'
import { ChatPage } from '../pages/ChatPage'
import { CreateWorkspacePage } from '../pages/CreateWorkspacePage'
import { DashboardPage } from '../pages/DashboardPage'
import { FeatureDetailPage } from '../pages/FeatureDetailPage'
import { InsightsPage } from '../pages/InsightsPage'
import { KnowledgeItemPage } from '../pages/KnowledgeItemPage'
import { NotFoundPage } from '../pages/NotFoundPage'
import { ProjectDetailsPage } from '../pages/ProjectDetailsPage'
import { SettingsPage } from '../pages/SettingsPage'
import { WorkItemPage } from '../pages/WorkItemPage'
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
                    path="/projects/:projectId/conversations/:conversationId"
                    element={<ChatPage />}
                />

                <Route
                    path="/insights"
                    element={<InsightsPage />}
                />

                <Route
                    path="/knowledge/:knowledgeId"
                    element={<KnowledgeItemPage />}
                />

                <Route
                    path="/developer/features/:featureId"
                    element={<FeatureDetailPage />}
                />

                <Route
                    path="/workitems/:workItemId"
                    element={<WorkItemPage />}
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