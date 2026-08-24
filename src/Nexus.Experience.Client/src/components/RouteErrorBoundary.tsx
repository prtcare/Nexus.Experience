import { Component, type ErrorInfo, type ReactNode } from 'react'

interface RouteErrorBoundaryProps {
    children: ReactNode
}

interface RouteErrorBoundaryState {
    error: Error | null
}

// Catches a render crash in whatever page is currently mounted at
// <Outlet /> so it doesn't blank the whole app - the sidebar and header
// stay up, only the page slot fails. AppLayout remounts this with the
// route's pathname as a key, so navigating away from a crashed page
// recovers automatically instead of leaving the fallback stuck forever.
export class RouteErrorBoundary extends Component<
    RouteErrorBoundaryProps,
    RouteErrorBoundaryState
> {
    state: RouteErrorBoundaryState = { error: null }

    static getDerivedStateFromError(
        error: Error,
    ): RouteErrorBoundaryState {
        return { error }
    }

    componentDidCatch(error: Error, info: ErrorInfo) {
        console.error(
            'Route render crashed:',
            error,
            info.componentStack,
        )
    }

    render() {
        if (this.state.error) {
            return (
                <div className="nexus-empty-state nexus-route-error">
                    <strong>
                        This page hit an error and couldn't
                        render.
                    </strong>

                    <p>{this.state.error.message}</p>
                </div>
            )
        }

        return this.props.children
    }
}
