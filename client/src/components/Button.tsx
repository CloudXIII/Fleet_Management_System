import clsx from 'clsx'
import type { ButtonHTMLAttributes, PropsWithChildren } from 'react'

type Variant = 'primary' | 'secondary' | 'ghost' | 'danger'

const styles: Record<Variant, string> = {
  primary:
    'bg-brand text-white hover:bg-[#0e4f3b] shadow-sm shadow-brand/20',
  secondary:
    'bg-panel text-ink border border-line hover:bg-brand-soft',
  ghost: 'bg-transparent text-ink-muted hover:bg-brand-soft hover:text-ink',
  danger: 'bg-danger-soft text-danger hover:bg-[#fecdca]',
}

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: Variant
}

export function Button({
  variant = 'primary',
  className,
  children,
  ...props
}: PropsWithChildren<ButtonProps>) {
  return (
    <button
      className={clsx(
        'inline-flex items-center justify-center gap-2 rounded-lg px-3.5 py-2 text-sm font-medium transition disabled:cursor-not-allowed disabled:opacity-50',
        styles[variant],
        className,
      )}
      {...props}
    >
      {children}
    </button>
  )
}
