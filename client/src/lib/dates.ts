import { format, parseISO } from 'date-fns'

export function formatDate(value?: string | null) {
  if (!value) return '—'
  try {
    return format(parseISO(value), 'dd MMM yyyy')
  } catch {
    return value
  }
}

export function toDateInput(value?: string | null) {
  if (!value) return ''
  return value.slice(0, 10)
}
