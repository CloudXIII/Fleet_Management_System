export type InsuranceStatus = 'Valid' | 'ExpiringSoon' | 'Expired'

export interface Vehicle {
  id: number
  plate: string
  brand: string
  model: string
  insuranceExpiry: string
  insuranceStatus: InsuranceStatus
  componentCount: number
  activeDriver?: string | null
}

export interface VehicleWrite {
  plate: string
  brand: string
  model: string
  insuranceExpiry: string
}

export interface VehicleComponent {
  id: number
  name: string
  serialNumber: string
  installationDate: string
  vehicleId: number
}

export interface VehicleComponentWrite {
  name: string
  serialNumber: string
  installationDate: string
  vehicleId: number
}

export interface DriverAssignment {
  id: number
  driverName: string
  startDate: string
  endDate?: string | null
  vehicleId: number
  isActive: boolean
}

export interface DriverAssignmentWrite {
  driverName: string
  startDate: string
  endDate?: string | null
  vehicleId: number
}

export interface Dashboard {
  totalVehicles: number
  activeAssignments: number
  expiringInsuranceCount: number
  expiredInsuranceCount: number
  totalComponents: number
  vehiclesByBrand: { brand: string; count: number }[]
  insuranceAlerts: {
    id: number
    plate: string
    brand: string
    model: string
    insuranceExpiry: string
    insuranceStatus: InsuranceStatus
  }[]
  recentAssignments: {
    id: number
    driverName: string
    vehiclePlate: string
    startDate: string
    isActive: boolean
  }[]
}
