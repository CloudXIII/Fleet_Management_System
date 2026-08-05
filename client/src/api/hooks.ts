import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { api } from './client'
import type {
  Dashboard,
  DriverAssignment,
  DriverAssignmentWrite,
  Vehicle,
  VehicleComponent,
  VehicleComponentWrite,
  VehicleWrite,
} from '../types'

export const queryKeys = {
  dashboard: ['dashboard'] as const,
  vehicles: ['vehicles'] as const,
  vehicle: (id: number) => ['vehicles', id] as const,
  components: (vehicleId: number) => ['components', vehicleId] as const,
  assignments: (vehicleId: number) => ['assignments', vehicleId] as const,
}

export function useDashboard() {
  return useQuery({
    queryKey: queryKeys.dashboard,
    queryFn: () => api.get<Dashboard>('/api/dashboard'),
  })
}

export function useVehicles() {
  return useQuery({
    queryKey: queryKeys.vehicles,
    queryFn: () => api.get<Vehicle[]>('/api/vehicles'),
  })
}

export function useVehicle(id: number) {
  return useQuery({
    queryKey: queryKeys.vehicle(id),
    queryFn: () => api.get<Vehicle>(`/api/vehicles/${id}`),
    enabled: Number.isFinite(id),
  })
}

export function useVehicleMutations() {
  const qc = useQueryClient()
  const invalidate = () => {
    void qc.invalidateQueries({ queryKey: queryKeys.vehicles })
    void qc.invalidateQueries({ queryKey: queryKeys.dashboard })
  }

  const create = useMutation({
    mutationFn: (body: VehicleWrite) => api.post<Vehicle>('/api/vehicles', body),
    onSuccess: invalidate,
  })

  const update = useMutation({
    mutationFn: ({ id, body }: { id: number; body: VehicleWrite }) =>
      api.put<Vehicle>(`/api/vehicles/${id}`, body),
    onSuccess: (vehicle) => {
      invalidate()
      void qc.invalidateQueries({ queryKey: queryKeys.vehicle(vehicle.id) })
    },
  })

  const remove = useMutation({
    mutationFn: (id: number) => api.delete(`/api/vehicles/${id}`),
    onSuccess: invalidate,
  })

  return { create, update, remove }
}

export function useComponents(vehicleId: number) {
  return useQuery({
    queryKey: queryKeys.components(vehicleId),
    queryFn: () =>
      api.get<VehicleComponent[]>(`/api/vehicle-components?vehicleId=${vehicleId}`),
    enabled: Number.isFinite(vehicleId),
  })
}

export function useComponentMutations(vehicleId: number) {
  const qc = useQueryClient()
  const invalidate = () => {
    void qc.invalidateQueries({ queryKey: queryKeys.components(vehicleId) })
    void qc.invalidateQueries({ queryKey: queryKeys.vehicle(vehicleId) })
    void qc.invalidateQueries({ queryKey: queryKeys.vehicles })
    void qc.invalidateQueries({ queryKey: queryKeys.dashboard })
  }

  return {
    create: useMutation({
      mutationFn: (body: VehicleComponentWrite) =>
        api.post<VehicleComponent>('/api/vehicle-components', body),
      onSuccess: invalidate,
    }),
    update: useMutation({
      mutationFn: ({ id, body }: { id: number; body: VehicleComponentWrite }) =>
        api.put<VehicleComponent>(`/api/vehicle-components/${id}`, body),
      onSuccess: invalidate,
    }),
    remove: useMutation({
      mutationFn: (id: number) => api.delete(`/api/vehicle-components/${id}`),
      onSuccess: invalidate,
    }),
  }
}

export function useAssignments(vehicleId: number) {
  return useQuery({
    queryKey: queryKeys.assignments(vehicleId),
    queryFn: () =>
      api.get<DriverAssignment[]>(`/api/driver-assignments?vehicleId=${vehicleId}`),
    enabled: Number.isFinite(vehicleId),
  })
}

export function useAssignmentMutations(vehicleId: number) {
  const qc = useQueryClient()
  const invalidate = () => {
    void qc.invalidateQueries({ queryKey: queryKeys.assignments(vehicleId) })
    void qc.invalidateQueries({ queryKey: queryKeys.vehicle(vehicleId) })
    void qc.invalidateQueries({ queryKey: queryKeys.vehicles })
    void qc.invalidateQueries({ queryKey: queryKeys.dashboard })
  }

  return {
    create: useMutation({
      mutationFn: (body: DriverAssignmentWrite) =>
        api.post<DriverAssignment>('/api/driver-assignments', body),
      onSuccess: invalidate,
    }),
    update: useMutation({
      mutationFn: ({ id, body }: { id: number; body: DriverAssignmentWrite }) =>
        api.put<DriverAssignment>(`/api/driver-assignments/${id}`, body),
      onSuccess: invalidate,
    }),
    remove: useMutation({
      mutationFn: (id: number) => api.delete(`/api/driver-assignments/${id}`),
      onSuccess: invalidate,
    }),
  }
}
