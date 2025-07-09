import subprocess
import os
import signal
import time

# Paths relativos o absolutos a tus proyectos
backend1_path = r"C:\Users\salvador.castro.ASPIRESYS\Desktop\e-commerce_rest\e-commerce_backend\authenticationwebapi"
backend2_path = r"C:\Users\salvador.castro.ASPIRESYS\Desktop\e-commerce_rest\e-commerce_backend\productswebapi"
frontend_path = r"C:\Users\salvador.castro.ASPIRESYS\Desktop\e-commerce_rest\ecommerce_frontend"

# Comandos para abrir terminales
proc1 = subprocess.Popen(f'start "" cmd /k "dotnet run"', cwd=backend1_path, shell=True)
proc2 = subprocess.Popen(f'start "" cmd /k "dotnet run"', cwd=backend2_path, shell=True)
proc3 = subprocess.Popen(f'start "" cmd /k "ng serve -o"', cwd=frontend_path, shell=True)

print("\n🚀 Aplicación lanzada. Presiona ENTER para cerrar todas las terminales...")
input()

# Mata todas las terminales CMD abiertas por este script
print("⛔ Cerrando procesos...")
os.system('taskkill /F /FI "WINDOWTITLE eq C:\\WINDOWS\\system32\\cmd.exe"')

print("✅ Aplicación detenida.")
