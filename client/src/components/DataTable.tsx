import {
  flexRender,
  getCoreRowModel,
  getFilteredRowModel,
  getSortedRowModel,
  useReactTable,
  type ColumnDef,
  type SortingState,
} from '@tanstack/react-table'
import { useState } from 'react'
import { ArrowDownWideNarrow, ArrowUpNarrowWide } from 'lucide-react'

interface DataTableProps<T> {
  data: T[]
  columns: ColumnDef<T, unknown>[]
  onRowClick?: (row: T) => void
  emptyMessage?: string
}

export function DataTable<T>({
  data,
  columns,
  onRowClick,
  emptyMessage = 'No records found.',
}: DataTableProps<T>) {
  const [sorting, setSorting] = useState<SortingState>([])
  const [globalFilter, setGlobalFilter] = useState('')

  const table = useReactTable({
    data,
    columns,
    state: { sorting, globalFilter },
    onSortingChange: setSorting,
    onGlobalFilterChange: setGlobalFilter,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
  })

  return (
    <div className="space-y-3">
      <div className="flex justify-end">
        <input
          value={globalFilter}
          onChange={(e) => setGlobalFilter(e.target.value)}
          placeholder="Search…"
          className="w-full max-w-xs rounded-lg border border-line bg-panel px-3 py-2 text-sm outline-none focus:border-brand focus:ring-2 focus:ring-brand/15"
        />
      </div>
      <div className="overflow-x-auto rounded-xl border border-line bg-panel">
        <table className="min-w-full text-left text-sm">
          <thead className="bg-brand-soft/60 text-ink-muted">
            {table.getHeaderGroups().map((headerGroup) => (
              <tr key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <th key={header.id} className="px-4 py-3 font-semibold">
                    {header.isPlaceholder ? null : (
                      <button
                        type="button"
                        className="inline-flex items-center gap-1.5"
                        onClick={header.column.getToggleSortingHandler()}
                      >
                        {flexRender(header.column.columnDef.header, header.getContext())}
                        {{
                          asc: <ArrowUpNarrowWide size={14} />,
                          desc: <ArrowDownWideNarrow size={14} />,
                        }[header.column.getIsSorted() as string] ?? null}
                      </button>
                    )}
                  </th>
                ))}
              </tr>
            ))}
          </thead>
          <tbody>
            {table.getRowModel().rows.length === 0 ? (
              <tr>
                <td colSpan={columns.length} className="px-4 py-10 text-center text-ink-muted">
                  {emptyMessage}
                </td>
              </tr>
            ) : (
              table.getRowModel().rows.map((row) => (
                <tr
                  key={row.id}
                  onClick={() => onRowClick?.(row.original)}
                  className={
                    onRowClick
                      ? 'cursor-pointer border-t border-line transition hover:bg-brand-soft/40'
                      : 'border-t border-line'
                  }
                >
                  {row.getVisibleCells().map((cell) => (
                    <td key={cell.id} className="px-4 py-3 align-middle">
                      {flexRender(cell.column.columnDef.cell, cell.getContext())}
                    </td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  )
}
