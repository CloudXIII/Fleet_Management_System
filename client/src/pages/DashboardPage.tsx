import { Link } from 'react-router-dom'
import {
  AlertTriangle,
  Boxes,
  ShieldAlert,
  Truck,
  UserCheck,
} from 'lucide-react'
import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts'
import { useDashboard } from '../api/hooks'
import { StatusBadge, ActiveBadge } from '../components/StatusBadge'
import { formatDate } from '../lib/dates'

function StatCard({
  label,
  value,
  icon: Icon,
  tone,
  delay,
}: {
  label: string
  value: number
  icon: typeof Truck
  tone: 'brand' | 'accent' | 'danger' | 'ok'
  delay: string
}) {
  const tones = {
    brand: 'bg-brand-soft text-brand',
    accent: 'bg-accent-soft text-accent',
    danger: 'bg-danger-soft text-danger',
    ok: 'bg-ok-soft text-ok',
  }

  return (
    <div className={`animate-fade-up ${delay} rounded-2xl border border-line bg-panel p-4 shadow-sm`}>
      <div className="flex items-start justify-between gap-3">
        <div>
          <p className="text-xs font-semibold uppercase tracking-wide text-ink-muted">{label}</p>
          <p className="mt-2 font-display text-3xl font-bold text-ink">{value}</p>
        </div>
        <span className={`rounded-xl p-2.5 ${tones[tone]}`}>
          <Icon size={20} />
        </span>
      </div>
    </div>
  )
}

export function DashboardPage() {
  const { data, isLoading, error } = useDashboard()

  if (isLoading) {
    return <p className="text-ink-muted">Loading fleet overview…</p>
  }

  if (error || !data) {
    return <p className="text-danger">Failed to load dashboard.</p>
  }

  return (
    <div className="space-y-6">
      <header className="animate-fade-up">
        <h1 className="font-display text-3xl font-bold text-ink">Fleet overview</h1>
        <p className="mt-1 max-w-2xl text-ink-muted">
          Monitor insurance risk, active drivers, and brand mix across the fleet.
        </p>
      </header>

      <section className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        <StatCard label="Vehicles" value={data.totalVehicles} icon={Truck} tone="brand" delay="stagger-1" />
        <StatCard label="Active drivers" value={data.activeAssignments} icon={UserCheck} tone="ok" delay="stagger-2" />
        <StatCard label="Expiring soon" value={data.expiringInsuranceCount} icon={AlertTriangle} tone="accent" delay="stagger-3" />
        <StatCard label="Expired" value={data.expiredInsuranceCount} icon={ShieldAlert} tone="danger" delay="stagger-4" />
      </section>

      <section className="grid gap-4 lg:grid-cols-5">
        <div className="animate-fade-up stagger-2 rounded-2xl border border-line bg-panel p-5 shadow-sm lg:col-span-3">
          <div className="mb-4 flex items-center justify-between">
            <h2 className="font-display text-lg font-semibold">Vehicles by brand</h2>
            <span className="inline-flex items-center gap-1 text-xs text-ink-muted">
              <Boxes size={14} /> {data.totalComponents} components
            </span>
          </div>
          <div className="h-64">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={data.vehiclesByBrand}>
                <CartesianGrid strokeDasharray="3 3" stroke="#d5e0db" vertical={false} />
                <XAxis dataKey="brand" tick={{ fill: '#4a635a', fontSize: 12 }} axisLine={false} tickLine={false} />
                <YAxis allowDecimals={false} tick={{ fill: '#4a635a', fontSize: 12 }} axisLine={false} tickLine={false} />
                <Tooltip
                  cursor={{ fill: 'rgba(11,61,46,0.06)' }}
                  contentStyle={{ borderRadius: 12, borderColor: '#d5e0db' }}
                />
                <Bar dataKey="count" fill="#0b3d2e" radius={[8, 8, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="animate-fade-up stagger-3 rounded-2xl border border-line bg-panel p-5 shadow-sm lg:col-span-2">
          <h2 className="font-display text-lg font-semibold">Insurance alerts</h2>
          <ul className="mt-4 space-y-3">
            {data.insuranceAlerts.length === 0 ? (
              <li className="text-sm text-ink-muted">No insurance issues right now.</li>
            ) : (
              data.insuranceAlerts.map((alert) => (
                <li key={alert.id}>
                  <Link
                    to={`/vehicles/${alert.id}`}
                    className="block rounded-xl border border-line px-3 py-2.5 transition hover:border-brand hover:bg-brand-soft/50"
                  >
                    <div className="flex items-center justify-between gap-2">
                      <p className="font-semibold text-ink">{alert.plate}</p>
                      <StatusBadge status={alert.insuranceStatus} />
                    </div>
                    <p className="mt-1 text-sm text-ink-muted">
                      {alert.brand} {alert.model} · {formatDate(alert.insuranceExpiry)}
                    </p>
                  </Link>
                </li>
              ))
            )}
          </ul>
        </div>
      </section>

      <section className="animate-fade-up stagger-4 rounded-2xl border border-line bg-panel p-5 shadow-sm">
        <h2 className="font-display text-lg font-semibold">Recent assignments</h2>
        <div className="mt-4 divide-y divide-line">
          {data.recentAssignments.map((item) => (
            <div key={item.id} className="flex flex-wrap items-center justify-between gap-2 py-3">
              <div>
                <p className="font-medium text-ink">{item.driverName}</p>
                <p className="text-sm text-ink-muted">
                  {item.vehiclePlate} · since {formatDate(item.startDate)}
                </p>
              </div>
              <ActiveBadge active={item.isActive} />
            </div>
          ))}
        </div>
      </section>
    </div>
  )
}
