import cv2
import numpy as np

# Abrir la imagen combinada
imagen = cv2.imread("Imagenes/combinada.jpg")

# Estado inicial de los canales (todos visibles)
canales = [True, True, True]  # [Azul, Verde, Rojo]

print("Controles:")
print("Tecla 'r' - alternar canal Rojo")
print("Tecla 'g' - alternar canal Verde")
print("Tecla 'b' - alternar canal Azul")
print("Tecla 'q' - salir")

while True:
    # Copiar la imagen para modificarla
    resultado = imagen.copy()

    # Apagar canales segun el estado
    if not canales[0]:  # Azul
        resultado[:, :, 0] = 0
    if not canales[1]:  # Verde
        resultado[:, :, 1] = 0
    if not canales[2]:  # Rojo
        resultado[:, :, 2] = 0

    cv2.imshow("Canales de Color - r/g/b para alternar", resultado)

    tecla = cv2.waitKey(1) & 0xFF

    if tecla == ord('r'):
        canales[2] = not canales[2]
        print("Canal Rojo:", "visible" if canales[2] else "oculto")
    elif tecla == ord('g'):
        canales[1] = not canales[1]
        print("Canal Verde:", "visible" if canales[1] else "oculto")
    elif tecla == ord('b'):
        canales[0] = not canales[0]
        print("Canal Azul:", "visible" if canales[0] else "oculto")
    elif tecla == ord('q'):
        break

cv2.destroyAllWindows()