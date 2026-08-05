import type { InsuranceStatus } from '../types'
import clsx from 'clsx'

const map: Record<InsuranceStatus, { label: string; className: string }> = {
  Valid: { label: 'Valid', className: 'bg-ok-soft text-ok' },
  ExpiringSoon: { label: 'Expiring soon', className: 'bg-accent-soft text-accent' },
  Expired: { label: 'Expired', className: 'bg-danger-soft text-danger' },
}

export function StatusBadge({ status }: { status: InsuranceStatus }) {
  const item = map[status]
  return (
    <span
      className={clsx(
        'inline-flex items-center rounded-md px-2 py-0.5 text-xs font-semibold tracking-wide',
        item.className,
      )}
    >
      {item.label}
    </span>
  )
}

export function ActiveBadge({ active }: { active: boolean }) {
  return (
    <span
      className={clsx(
        'inline-flex items-center rounded-md px-2 py-0.5 text-xs font-semibold',
        active ? 'bg-ok-soft text-ok' : 'bg-surface text-ink-muted',
      )}
    >
      {active ? 'Active' : 'Ended'}
    </span>
  )
}
