import type { ReactNode } from 'react'

type CardProps = {
    title?: string
    children: ReactNode
    className?: string
}

export function Card({
    title,
    children,
    className = '',
}: CardProps) {
    return (
        <section className={`nexus-card ${className}`}>
            {title && (
                <header className="nexus-card-header">
                    <h2>{title}</h2>
                </header>
            )}

            <div className="nexus-card-content">
                {children}
            </div>
        </section>
    )
}