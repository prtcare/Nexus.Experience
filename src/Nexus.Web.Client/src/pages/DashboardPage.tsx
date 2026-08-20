import { Card } from '../components/Card'
import { MetricCard } from '../components/MetricCard'
import { useSystemHealth } from '../features/system/useSystemHealth'
import { useWorkspaces } from '../features/workspaces/useWorkspaces'
import { useNavigate } from 'react-router-dom'
export function DashboardPage() {
    const {
        data: systemHealth,
        isPending: isSystemPending,
        isError: isSystemError,
    } = useSystemHealth()

    const systemStatus = isSystemPending
        ? 'Checking'
        : isSystemError
            ? 'Unavailable'
            : systemHealth?.status ?? 'Unknown'

    const systemDescription = isSystemPending
        ? 'Checking Nexus API'
        : isSystemError
            ? 'Unable to reach Nexus API'
            : `${systemHealth?.service ?? 'Nexus API'} is operational`

    const {
        data: workspaceData,
        isPending: workspacesPending,
    } = useWorkspaces()

    const workspaceCount =
        workspaceData?.workspaces.length ?? 0
    const navigate = useNavigate()

    return (
        <div className="nexus-dashboard">
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Overview
                    </span>

                    <h1>Dashboard</h1>

                    <p>
                        Monitor Nexus products, insights,
                        and system activity.
                    </p>
                </div>

                <button
                    type="button"
                    className="nexus-primary-button"
                    onClick={() =>
                        navigate('/workspaces/new')
                    }
                >
                    New workspace
                </button>
            </header>

            <section className="nexus-metrics-grid">
                <MetricCard
                    label="Products"
                    value="—"
                    description="Available Nexus products"
                />

                <MetricCard
                    label="Insights"
                    value="—"
                    description="Active insight sessions"
                />

                <MetricCard
                    label="Workspaces"
                    value={
                        workspacesPending
                            ? '—'
                            : workspaceCount.toString()
                    }
                    description="Configured workspaces"
                />

                <MetricCard
                    label="System"
                    value={systemStatus}
                    description={systemDescription}
                />
            </section>

            <section className="nexus-dashboard-grid">
                <Card title="Recent activity">
                    <div className="nexus-empty-state">
                        <strong>No recent activity</strong>

                        <p>
                            Nexus activity will appear here
                            when API integration is enabled.
                        </p>
                    </div>
                </Card>

                <Card title="System status">
                    <div className="nexus-status-list">
                        <div className="nexus-status-row">
                            <div>
                                <strong>Nexus API</strong>
                                <span>Backend connectivity</span>
                            </div>

                            <span
                                className={
                                    isSystemPending
                                        ? 'nexus-status nexus-status-pending'
                                        : isSystemError
                                            ? 'nexus-status nexus-status-error'
                                            : 'nexus-status nexus-status-ok'
                                }
                            >
                                {isSystemPending
                                    ? 'Checking'
                                    : isSystemError
                                        ? 'Unavailable'
                                        : 'Operational'}
                            </span>
                        </div>

                        <div className="nexus-status-row">
                            <div>
                                <strong>Products API</strong>
                                <span>Backend connectivity</span>
                            </div>

                            <span className="nexus-status nexus-status-pending">
                                Not connected
                            </span>
                        </div>

                        <div className="nexus-status-row">
                            <div>
                                <strong>Insights</strong>
                                <span>AI services</span>
                            </div>

                            <span className="nexus-status nexus-status-pending">
                                Not connected
                            </span>
                        </div>
                    </div>
                </Card>
            </section>
        </div>
    )
}