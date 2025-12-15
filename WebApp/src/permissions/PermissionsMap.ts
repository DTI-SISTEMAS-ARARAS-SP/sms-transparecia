export const PermissionsMap = {
  ROOT: "root",
  USERS: "users",
  RESOURCES: "resources",
  REPORTS: "reports",
  PROFILE: "profile",
  CONVENIOS: "convenios",
} as const;

export type PermissionName =
  (typeof PermissionsMap)[keyof typeof PermissionsMap];
