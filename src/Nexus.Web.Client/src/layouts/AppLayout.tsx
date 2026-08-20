import { NavLink, Outlet } from 'react-router-dom'

export function AppLayout() {
    return (
        <div className="nexus-app">
            <aside className="nexus-sidebar">
                <div className="nexus-brand">
                    <div className="nexus-brand-mark">
                        N
                    </div>

                    <div>
                        <strong>Nexus</strong>
                        <span>Chat</span>
                    </div>
                </div>

                <nav className="nexus-navigation">
                    <NavLink to="/dashboard">
                        Dashboard
                    </NavLink>
                    <NavLink to="/workspaces">
                        Workspaces
                    </NavLink>
                    <NavLink to="/insights">
                        Insights
                    </NavLink>

                    <NavLink to="/settings">
                        Settings
                    </NavLink>
                </nav>
            </aside>

            <div className="nexus-shell">
                <header className="nexus-header">
                    <span className="nexus-header-title">
                        Nexus
                    </span>

                    <div className="nexus-user">
                        User
                    </div>
                </header>

                <main className="nexus-main">
                    <Outlet />
                </main>
            </div>
        </div>
    )
}