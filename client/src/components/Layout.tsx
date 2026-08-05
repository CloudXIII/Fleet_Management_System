import { NavLink, Outlet } from 'react-router-dom'
import { LayoutDashboard, Truck } from 'lucide-react'
import clsx from 'clsx'

const links = [
  { to: '/', label: 'Dashboard', icon: LayoutDashboard, end: true },
  { to: '/vehicles', label: 'Vehicles', icon: Truck, end: false },
]

export function AppLayout() {
  return (
    <div className="mx-auto flex min-h-screen max-w-7xl flex-col gap-6 px-4 py-5 md:px-8 md:py-8 lg:flex-row">
      <aside className="animate-fade-up shrink-0 lg:w-60">
        <div className="sticky top-5 space-y-6 rounded-2xl border border-line bg-panel/90 p-5 shadow-sm backdrop-blur">
          <div>
            <p className="font-display text-xl font-bold text-brand">Fleet Manager</p>
            <p className="mt-1 text-sm text-ink-muted">
              Operations console for vehicles, parts, and drivers.
            </p>
          </div>
          <nav className="space-y-1">
            {links.map(({ to, label, icon: Icon, end }) => (
              <NavLink
                key={to}
                to={to}
                end={end}
                className={({ isActive }) =>
                  clsx(
                    'flex items-center gap-2.5 rounded-xl px-3 py-2.5 text-sm font-medium transition',
                    isActive
                      ? 'bg-brand text-white shadow-sm'
                      : 'text-ink-muted hover:bg-brand-soft hover:text-ink',
                  )
                }
              >
                <Icon size={18} />
                {label}
              </NavLink>
            ))}
          </nav>
          <p className="border-t border-line pt-4 text-xs text-ink-muted">
            ASP.NET Core API · React · TanStack · Recharts
          </p>
        </div>
      </aside>
      <main className="min-w-0 flex-1 pb-10">
        <Outlet />
      </main>
    </div>
  )
}
