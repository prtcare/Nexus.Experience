import { Card } from '../components/Card'
import { MetricCard } from '../components/MetricCard'

export function DashboardPage() {
    return (
        <div className="nexus-dashboard">
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Overview
                    </span>

                    <h1>Dashboard</h1>

                    <p>
                        Monitor Nexus products, intelligence,
                        and platform activity.
                    </p>
                </div>

                <button
                    type="button"
                    className="nexus-primary-button"
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
                    label="Intelligence"
                    value="—"
                    description="Active intelligence sessions"
                />

                <MetricCard
                    label="Workspaces"
                    value="—"
                    description="Configured workspaces"
                />

                <MetricCard
                    label="Platform"
                    value="Ready"
                    description="Nexus Web is operational"
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

                <Card title="Platform status">
                    <div className="nexus-status-list">
                        <div className="nexus-status-row">
                            <div>
                                <strong>Nexus Web</strong>
                                <span>Frontend application</span>
                            </div>

                            <span className="nexus-status nexus-status-ok">
                                Operational
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
                                <strong>Intelligence</strong>
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