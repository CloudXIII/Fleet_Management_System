import { useMemo, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { type ColumnDef } from '@tanstack/react-table'
import { ArrowLeft, Pencil, Plus, Trash2 } from 'lucide-react'
import {
  useAssignmentMutations,
  useAssignments,
  useComponentMutations,
  useComponents,
  useVehicle,
} from '../api/hooks'
import { Button } from '../components/Button'
import { DataTable } from '../components/DataTable'
import { Field, Modal, inputClassName } from '../components/Modal'
import { ActiveBadge, StatusBadge } from '../components/StatusBadge'
import { formatDate, toDateInput } from '../lib/dates'
import type {
  DriverAssignment,
  DriverAssignmentWrite,
  VehicleComponent,
  VehicleComponentWrite,
} from '../types'
import clsx from 'clsx'

type Tab = 'components' | 'drivers'

export function VehicleDetailPage() {
  const { id } = useParams()
  const vehicleId = Number(id)
  const { data: vehicle, isLoading, error } = useVehicle(vehicleId)
  const [tab, setTab] = useState<Tab>('components')

  if (isLoading) return <p className="text-ink-muted">Loading vehicle…</p>
  if (error || !vehicle) return <p className="text-danger">Vehicle not found.</p>

  return (
    <div className="space-y-6">
      <div className="animate-fade-up">
        <Link
          to="/vehicles"
          className="inline-flex items-center gap-1.5 text-sm font-medium text-ink-muted transition hover:text-brand"
        >
          <ArrowLeft size={16} /> Back to vehicles
        </Link>
        <div className="mt-3 flex flex-wrap items-end justify-between gap-3">
          <div>
            <h1 className="font-display text-3xl font-bold text-ink">
              {vehicle.brand} {vehicle.model}
            </h1>
            <p className="mt-1 font-semibold tracking-wide text-brand">{vehicle.plate}</p>
          </div>
          <StatusBadge status={vehicle.insuranceStatus} />
        </div>
      </div>

      <section className="animate-fade-up stagger-1 grid gap-3 rounded-2xl border border-line bg-panel p-5 shadow-sm sm:grid-cols-2 lg:grid-cols-4">
        <Info label="Brand" value={vehicle.brand} />
        <Info label="Model" value={vehicle.model} />
        <Info label="Insurance expiry" value={formatDate(vehicle.insuranceExpiry)} />
        <Info label="Active driver" value={vehicle.activeDriver ?? 'Unassigned'} />
      </section>

      <section className="animate-fade-up stagger-2 rounded-2xl border border-line bg-panel p-5 shadow-sm">
        <div className="mb-4 flex gap-2 border-b border-line pb-3">
          <TabButton active={tab === 'components'} onClick={() => setTab('components')}>
            Components
          </TabButton>
          <TabButton active={tab === 'drivers'} onClick={() => setTab('drivers')}>
            Driver history
          </TabButton>
        </div>
        {tab === 'components' ? (
          <ComponentsPanel vehicleId={vehicleId} />
        ) : (
          <DriversPanel vehicleId={vehicleId} />
        )}
      </section>
    </div>
  )
}

function Info({ label, value }: { label: string; value: string }) {
  return (
    <div>
      <p className="text-xs font-semibold uppercase tracking-wide text-ink-muted">{label}</p>
      <p className="mt-1 text-sm font-medium text-ink">{value}</p>
    </div>
  )
}

function TabButton({
  active,
  onClick,
  children,
}: {
  active: boolean
  onClick: () => void
  children: React.ReactNode
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={clsx(
        'rounded-lg px-3 py-1.5 text-sm font-semibold transition',
        active ? 'bg-brand text-white' : 'text-ink-muted hover:bg-brand-soft hover:text-ink',
      )}
    >
      {children}
    </button>
  )
}

function ComponentsPanel({ vehicleId }: { vehicleId: number }) {
  const { data = [], isLoading } = useComponents(vehicleId)
  const { create, update, remove } = useComponentMutations(vehicleId)
  const [open, setOpen] = useState(false)
  const [editingId, setEditingId] = useState<number | null>(null)
  const [form, setForm] = useState({ name: '', serialNumber: '', installationDate: '' })
  const [formError, setFormError] = useState<string | null>(null)

  const columns = useMemo<ColumnDef<VehicleComponent>[]>(
    () => [
      { accessorKey: 'name', header: 'Name' },
      { accessorKey: 'serialNumber', header: 'Serial' },
      {
        accessorKey: 'installationDate',
        header: 'Installed',
        cell: ({ getValue }) => formatDate(getValue<string>()),
      },
      {
        id: 'actions',
        header: '',
        cell: ({ row }) => (
          <div className="flex justify-end gap-1">
            <Button variant="ghost" className="!px-2" onClick={() => openEdit(row.original)}>
              <Pencil size={16} />
            </Button>
            <Button
              variant="ghost"
              className="!px-2 text-danger"
              onClick={() => void remove.mutateAsync(row.original.id)}
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
    setForm({ name: '', serialNumber: '', installationDate: '' })
    setFormError(null)
    setOpen(true)
  }

  function openEdit(item: VehicleComponent) {
    setEditingId(item.id)
    setForm({
      name: item.name,
      serialNumber: item.serialNumber,
      installationDate: toDateInput(item.installationDate),
    })
    setFormError(null)
    setOpen(true)
  }

  async function onSubmit() {
    if (!form.name || !form.installationDate) {
      setFormError('Name and installation date are required.')
      return
    }
    const body: VehicleComponentWrite = {
      name: form.name,
      serialNumber: form.serialNumber,
      installationDate: new Date(form.installationDate).toISOString(),
      vehicleId,
    }
    try {
      if (editingId) await update.mutateAsync({ id: editingId, body })
      else await create.mutateAsync(body)
      setOpen(false)
    } catch (err) {
      setFormError(err instanceof Error ? err.message : 'Save failed')
    }
  }

  return (
    <div className="space-y-4">
      <div className="flex justify-end">
        <Button onClick={openCreate}>
          <Plus size={16} /> Add component
        </Button>
      </div>
      {isLoading ? (
        <p className="text-ink-muted">Loading…</p>
      ) : (
        <DataTable data={data} columns={columns} emptyMessage="No components yet." />
      )}
      <Modal
        open={open}
        title={editingId ? 'Edit component' : 'Add component'}
        onClose={() => setOpen(false)}
        footer={
          <>
            <Button variant="secondary" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button onClick={() => void onSubmit()}>Save</Button>
          </>
        }
      >
        <Field label="Name">
          <input
            className={inputClassName}
            value={form.name}
            onChange={(e) => setForm((f) => ({ ...f, name: e.target.value }))}
          />
        </Field>
        <Field label="Serial number">
          <input
            className={inputClassName}
            value={form.serialNumber}
            onChange={(e) => setForm((f) => ({ ...f, serialNumber: e.target.value }))}
          />
        </Field>
        <Field label="Installation date">
          <input
            type="date"
            className={inputClassName}
            value={form.installationDate}
            onChange={(e) => setForm((f) => ({ ...f, installationDate: e.target.value }))}
          />
        </Field>
        {formError ? <p className="text-sm text-danger">{formError}</p> : null}
      </Modal>
    </div>
  )
}

function DriversPanel({ vehicleId }: { vehicleId: number }) {
  const { data = [], isLoading } = useAssignments(vehicleId)
  const { create, update, remove } = useAssignmentMutations(vehicleId)
  const [open, setOpen] = useState(false)
  const [editingId, setEditingId] = useState<number | null>(null)
  const [form, setForm] = useState({ driverName: '', startDate: '', endDate: '' })
  const [formError, setFormError] = useState<string | null>(null)

  const columns = useMemo<ColumnDef<DriverAssignment>[]>(
    () => [
      { accessorKey: 'driverName', header: 'Driver' },
      {
        accessorKey: 'startDate',
        header: 'Start',
        cell: ({ getValue }) => formatDate(getValue<string>()),
      },
      {
        accessorKey: 'endDate',
        header: 'End',
        cell: ({ getValue }) => formatDate(getValue<string | null>()),
      },
      {
        accessorKey: 'isActive',
        header: 'Status',
        cell: ({ row }) => <ActiveBadge active={row.original.isActive} />,
      },
      {
        id: 'actions',
        header: '',
        cell: ({ row }) => (
          <div className="flex justify-end gap-1">
            <Button variant="ghost" className="!px-2" onClick={() => openEdit(row.original)}>
              <Pencil size={16} />
            </Button>
            <Button
              variant="ghost"
              className="!px-2 text-danger"
              onClick={() => void remove.mutateAsync(row.original.id)}
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
    setForm({ driverName: '', startDate: '', endDate: '' })
    setFormError(null)
    setOpen(true)
  }

  function openEdit(item: DriverAssignment) {
    setEditingId(item.id)
    setForm({
      driverName: item.driverName,
      startDate: toDateInput(item.startDate),
      endDate: toDateInput(item.endDate),
    })
    setFormError(null)
    setOpen(true)
  }

  async function onSubmit() {
    if (!form.driverName || !form.startDate) {
      setFormError('Driver name and start date are required.')
      return
    }
    const body: DriverAssignmentWrite = {
      driverName: form.driverName,
      startDate: new Date(form.startDate).toISOString(),
      endDate: form.endDate ? new Date(form.endDate).toISOString() : null,
      vehicleId,
    }
    try {
      if (editingId) await update.mutateAsync({ id: editingId, body })
      else await create.mutateAsync(body)
      setOpen(false)
    } catch (err) {
      setFormError(err instanceof Error ? err.message : 'Save failed')
    }
  }

  return (
    <div className="space-y-4">
      <div className="flex justify-end">
        <Button onClick={openCreate}>
          <Plus size={16} /> Add assignment
        </Button>
      </div>
      {isLoading ? (
        <p className="text-ink-muted">Loading…</p>
      ) : (
        <DataTable data={data} columns={columns} emptyMessage="No driver assignments yet." />
      )}
      <Modal
        open={open}
        title={editingId ? 'Edit assignment' : 'Add assignment'}
        onClose={() => setOpen(false)}
        footer={
          <>
            <Button variant="secondary" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button onClick={() => void onSubmit()}>Save</Button>
          </>
        }
      >
        <Field label="Driver name">
          <input
            className={inputClassName}
            value={form.driverName}
            onChange={(e) => setForm((f) => ({ ...f, driverName: e.target.value }))}
          />
        </Field>
        <Field label="Start date">
          <input
            type="date"
            className={inputClassName}
            value={form.startDate}
            onChange={(e) => setForm((f) => ({ ...f, startDate: e.target.value }))}
          />
        </Field>
        <Field label="End date (optional)">
          <input
            type="date"
            className={inputClassName}
            value={form.endDate}
            onChange={(e) => setForm((f) => ({ ...f, endDate: e.target.value }))}
          />
        </Field>
        {formError ? <p className="text-sm text-danger">{formError}</p> : null}
      </Modal>
    </div>
  )
}
