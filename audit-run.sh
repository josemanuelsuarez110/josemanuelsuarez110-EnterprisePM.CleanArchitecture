#!/usr/bin/env bash
set -euo pipefail

# Simple orchestrator for common audit tools on Kali Linux.
# Outputs go to ./audit-reports by default (override with AUDIT_OUT_DIR).
# Usage: ./audit-run.sh [TARGET_URL] [CODE_DIR]
#   TARGET_URL defaults to http://localhost
#   CODE_DIR   defaults to current directory

TARGET_URL=${1:-http://localhost}
CODE_DIR=${2:-.}

TIMESTAMP=$(date -u +%Y%m%dT%H%M%SZ)
OUT_BASE=${AUDIT_OUT_DIR:-./audit-reports}
OUT_DIR="$OUT_BASE/audit-$TIMESTAMP"
mkdir -p "$OUT_DIR"

log() { printf "[%s] %s\n" "$(date -u +%H:%M:%S)" "$*"; }
warn() { printf "[%s] WARN: %s\n" "$(date -u +%H:%M:%S)" "$*" >&2; }

require_cmd() {
  for c in "$@"; do
    if ! command -v "$c" >/dev/null 2>&1; then
      warn "Falta el binario '$c'. Instala con apt/pip antes de continuar." && exit 1
    fi
  done
}

# Ensure core tools exist; ZAP/OpenVAS are optional and checked later.
require_cmd lynis trivy semgrep nikto sqlmap

run_and_capture() {
  local name=$1
  shift
  local logfile="$OUT_DIR/$name.log"
  log "==> $name"
  if ! "$@" | tee "$logfile"; then
    warn "$name falló; revisa $logfile"
  fi
}

# 1) Lynis (requiere sudo)
if sudo -n true 2>/dev/null; then
  run_and_capture lynis sudo lynis audit system
else
  warn "Lynis requiere sudo con NOPASSWD para ejecución no interactiva; saltando."
fi

# 2) Trivy (filesystem / contenedor)
run_and_capture trivy trivy fs --quiet --severity HIGH,CRITICAL --timeout 15m --skip-dirs /proc,/sys,/dev "$CODE_DIR"

# 3) Semgrep (SAST)
run_and_capture semgrep semgrep --config auto "$CODE_DIR"

# 4) Nikto (web)
run_and_capture nikto nikto -h "$TARGET_URL"

# 5) sqlmap (inyección SQL básica)
run_and_capture sqlmap sqlmap -u "$TARGET_URL" --batch --crawl=1 --level=1 --risk=1

# 6) ZAP baseline scan si está disponible
if command -v zap.sh >/dev/null 2>&1; then
  ZAP_OUT="$OUT_DIR/zap-baseline.html"
  log "==> zap-baseline"
  if /usr/share/zaproxy/zap.sh -cmd -quickurl "$TARGET_URL" -quickout "$ZAP_OUT" -quickprogress; then
    log "ZAP baseline guardado en $ZAP_OUT"
  else
    warn "ZAP baseline falló; revisa $ZAP_OUT"
  fi
else
  warn "ZAP no instalado (zaproxy); omitiendo paso ZAP."
fi

# 7) OpenVAS / GVM (opcional, pesado)
if command -v gvm-cli >/dev/null 2>&1; then
  warn "GVM/OpenVAS requiere tareas y credenciales ya configuradas; ejecútalo manualmente si lo necesitas."
fi

log "Listo. Reportes en $OUT_DIR"
