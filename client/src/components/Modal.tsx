import { useEffect } from 'react'
import { X } from 'lucide-react'
import { Button } from './Button'

interface ModalProps {
  open: boolean
  title: string
  onClose: () => void
  children: React.ReactNode
  footer?: React.ReactNode
}

export function Modal({ open, title, onClose, children, footer }: ModalProps) {
  useEffect(() => {
    if (!open) return
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
    }
    window.addEventListener('keydown', onKey)
    return () => window.removeEventListener('keydown', onKey)
  }, [open, onClose])

  if (!open) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 animate-fade-in">
      <button
        type="button"
        aria-label="Close dialog"
        className="absolute inset-0 bg-ink/40 backdrop-blur-[2px]"
        onClick={onClose}
      />
      <div
        role="dialog"
        aria-modal="true"
        className="relative z-10 w-full max-w-lg rounded-2xl border border-line bg-panel p-5 shadow-xl animate-fade-up"
      >
        <div className="mb-4 flex items-start justify-between gap-3">
          <h2 className="font-display text-lg font-semibold text-ink">{title}</h2>
          <Button variant="ghost" className="!px-2 !py-1" onClick={onClose} aria-label="Close">
            <X size={18} />
          </Button>
        </div>
        <div className="space-y-3">{children}</div>
        {footer ? <div className="mt-5 flex justify-end gap-2">{footer}</div> : null}
      </div>
    </div>
  )
}

export function Field({
  label,
  children,
}: {
  label: string
  children: React.ReactNode
}) {
  return (
    <label className="block space-y-1.5">
      <span className="text-xs font-semibold uppercase tracking-wide text-ink-muted">
        {label}
      </span>
      {children}
    </label>
  )
}

export const inputClassName =
  'w-full rounded-lg border border-line bg-surface px-3 py-2 text-sm text-ink outline-none transition focus:border-brand focus:bg-panel focus:ring-2 focus:ring-brand/15'
