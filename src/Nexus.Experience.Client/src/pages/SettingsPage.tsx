import { Card } from '../components/Card'
import { nexusEnvironment } from '../config/environment'

export function SettingsPage() {
    return (
        <div>
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Configuration
                    </span>

                    <h1>Settings</h1>

                    <p>
                        Nexus Web runtime and environment configuration.
                    </p>
                </div>
            </header>

            <Card title="Environment">
                <div className="nexus-settings-list">
                    <div className="nexus-settings-row">
                        <span>Environment</span>
                        <strong>
                            {nexusEnvironment.environment}
                        </strong>
                    </div>

                    <div className="nexus-settings-row">
                        <span>API URL</span>
                        <strong>
                            {nexusEnvironment.apiBaseUrl}
                        </strong>
                    </div>

                    <div className="nexus-settings-row">
                        <span>Mode</span>
                        <strong>
                            {nexusEnvironment.isDevelopment
                                ? 'Development'
                                : 'Production'}
                        </strong>
                    </div>
                </div>
            </Card>
        </div>
    )
}