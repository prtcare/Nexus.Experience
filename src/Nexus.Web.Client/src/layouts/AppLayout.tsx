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
                        <span>AI Platform</span>
                    </div>
                </div>

                <nav className="nexus-navigation">
                    <NavLink to="/dashboard">
                        Dashboard
                    </NavLink>

                    <NavLink to="/products">
                        Products
                    </NavLink>

                    <NavLink to="/intelligence">
                        Intelligence
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