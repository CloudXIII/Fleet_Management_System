import { useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { type ColumnDef } from '@tanstack/react-table'
import { Pencil, Plus, Trash2 } from 'lucide-react'
import { useVehicleMutations, useVehicles } from '../api/hooks'
import { Button } from '../components/Button'
import { DataTable } from '../components/DataTable'
import { Field, Modal, inputClassName } from '../components/Modal'
import { StatusBadge } from '../components/StatusBadge'
import { formatDate, toDateInput } from '../lib/dates'
import type { Vehicle, VehicleWrite } from '../types'

const emptyForm: VehicleWrite = {
  plate: '',
  brand: '',
  model: '',
  insuranceExpiry: '',
}

export function VehiclesPage() {
  const navigate = useNavigate()
  const { data = [], isLoading, error } = useVehicles()
  const { create, update, remove } = useVehicleMutations()
  const [form, setForm] = useState<VehicleWrite>(emptyForm)
  const [editingId, setEditingId] = useState<number | null>(null)
  const [open, setOpen] = useState(false)
  const [formError, setFormError] = useState<string | null>(null)

  const columns = useMemo<ColumnDef<Vehicle>[]>(
    () => [
      {
        accessorKey: 'plate',
        header: 'Plate',
        cell: ({ row }) => (
          <span className="font-semibold tracking-wide text-brand">{row.original.plate}</span>
        ),
      },
      { accessorKey: 'brand', header: 'Brand' },
      { accessorKey: 'model', header: 'Model' },
      {
        accessorKey: 'insuranceExpiry',
        header: 'Insurance',
        cell: ({ getValue }) => formatDate(getValue<string>()),
      },
      {
        accessorKey: 'insuranceStatus',
        header: 'Status',
        cell: ({ row }) => <StatusBadge status={row.original.insuranceStatus} />,
      },
      {
        accessorKey: 'activeDriver',
        header: 'Driver',
        cell: ({ getValue }) => getValue<string | null>() ?? '—',
      },
      {
        accessorKey: 'componentCount',
        header: 'Parts',
      },
      {
        id: 'actions',
        header: '',
        cell: ({ row }) => (
          <div className="flex justify-end gap-1" onClick={(e) => e.stopPropagation()}>
            <Button
              variant="ghost"
              className="!px-2"
              onClick={() => openEdit(row.original)}
              aria-label="Edit vehicle"
            >
              <Pencil size={16} />
            </Button>
            <Button
              variant="ghost"
              className="!px-2 text-danger"
              onClick={() => void onDelete(row.original.id)}
              aria-label="Delete vehicle"
            >
              <Trash2 size={16} />
            </Button>
          </div>
        ),
      },
    ],
    [],
  )

  function openCreate() {
    setEditingId(null)
    setForm(emptyForm)
    setFormError(null)
    setOpen(true)
  }

  function openEdit(vehicle: Vehicle) {
    setEditingId(vehicle.id)
    setForm({
      plate: vehicle.plate,
      brand: vehicle.brand,
      model: vehicle.model,
      insuranceExpiry: toDateInput(vehicle.insuranceExpiry),
    })
    setFormError(null)
    setOpen(true)
  }

  async function onDelete(id: number) {
    if (!window.confirm('Delete this vehicle and related records?')) return
    await remove.mutateAsync(id)
  }

  async function onSubmit() {
    if (!form.plate || !form.brand || !form.model || !form.insuranceExpiry) {
      setFormError('All fields are required.')
      return
    }

    const payload: VehicleWrite = {
      ...form,
      insuranceExpiry: new Date(form.insuranceExpiry).toISOString(),
    }

    try {
      if (editingId) {
        await update.mutateAsync({ id: editingId, body: payload })
      } else {
        await create.mutateAsync(payload)
      }
      setOpen(false)
    } catch (err) {
      setFormError(err instanceof Error ? err.message : 'Save failed')
    }
  }

  return (
    <div className="space-y-6">
      <header className="animate-fade-up flex flex-wrap items-end justify-between gap-3">
        <div>
          <h1 className="font-display text-3xl font-bold text-ink">Vehicles</h1>
          <p className="mt-1 text-ink-muted">
            Search, sort, and manage the fleet. Open a row for components and driver history.
          </p>
        </div>
        <Button onClick={openCreate}>
          <Plus size={16} /> Add vehicle
        </Button>
      </header>

      <div className="animate-fade-up stagger-2">
        {isLoading ? (
          <p className="text-ink-muted">Loading vehicles…</p>
        ) : error ? (
          <p className="text-danger">Failed to load vehicles.</p>
        ) : (
          <DataTable
            data={data}
            columns={columns}
            onRowClick={(row) => navigate(`/vehicles/${row.id}`)}
          />
        )}
      </div>

      <Modal
        open={open}
        title={editingId ? 'Edit vehicle' : 'Add vehicle'}
        onClose={() => setOpen(false)}
        footer={
          <>
            <Button variant="secondary" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button onClick={() => void onSubmit()} disabled={create.isPending || update.isPending}>
              Save
            </Button>
          </>
        }
      >
        <Field label="Plate">
          <input
            className={inputClassName}
            value={form.plate}
            onChange={(e) => setForm((f) => ({ ...f, plate: e.target.value }))}
          />
        </Field>
        <Field label="Brand">
          <input
            className={inputClassName}
            value={form.brand}
            onChange={(e) => setForm((f) => ({ ...f, brand: e.target.value }))}
          />
        </Field>
        <Field label="Model">
          <input
            className={inputClassName}
            value={form.model}
            onChange={(e) => setForm((f) => ({ ...f, model: e.target.value }))}
          />
        </Field>
        <Field label="Insurance expiry">
          <input
            type="date"
            className={inputClassName}
            value={form.insuranceExpiry}
            onChange={(e) => setForm((f) => ({ ...f, insuranceExpiry: e.target.value }))}
          />
        </Field>
        {formError ? <p className="text-sm text-danger">{formError}</p> : null}
      </Modal>
    </div>
  )
}
