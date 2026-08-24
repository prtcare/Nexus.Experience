import { Card } from './Card'

type MetricCardProps = {
    label: string
    value: string
    description: string
}

export function MetricCard({
    label,
    value,
    description,
}: MetricCardProps) {
    return (
        <Card className="nexus-metric-card">
            <span className="nexus-metric-label">
                {label}
            </span>

            <strong className="nexus-metric-value">
                {value}
            </strong>

            <span className="nexus-metric-description">
                {description}
            </span>
        </Card>
    )
}