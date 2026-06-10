import cv2
import numpy as np

# Crear un lienzo en blanco
lienzo = np.ones((600, 800, 3), dtype=np.uint8) * 255

# Lista para guardar el historial (para deshacer)
historial = []

# Figura actual seleccionada y punto de inicio
figura_actual = "rectangulo"
punto_inicio = None

print("Controles:")
print("Tecla 'r' - dibujar rectangulo")
print("Tecla 'c' - dibujar circulo")
print("Tecla 'l' - dibujar linea")
print("Tecla 'z' - deshacer")
print("Tecla 's' - guardar")
print("Tecla 'q' - salir")

def eventos_mouse(evento, x, y, flags, param):
    global lienzo, punto_inicio, historial

    if evento == cv2.EVENT_LBUTTONDOWN:
        punto_inicio = (x, y)
        historial.append(lienzo.copy())

    elif evento == cv2.EVENT_LBUTTONUP:
        if punto_inicio is not None:
            if figura_actual == "rectangulo":
                cv2.rectangle(lienzo, punto_inicio, (x, y), (0, 0, 255), 2)
            elif figura_actual == "circulo":
                radio = int(((x - punto_inicio[0])**2 + (y - punto_inicio[1])**2)**0.5)
                cv2.circle(lienzo, punto_inicio, radio, (0, 255, 0), 2)
            elif figura_actual == "linea":
                cv2.line(lienzo, punto_inicio, (x, y), (255, 0, 0), 2)
            punto_inicio = None

cv2.namedWindow("Dibujo")
cv2.setMouseCallback("Dibujo", eventos_mouse)

while True:
    cv2.imshow("Dibujo", lienzo)
    tecla = cv2.waitKey(1) & 0xFF

    if tecla == ord('r'):
        figura_actual = "rectangulo"
        print("Figura: rectangulo")
    elif tecla == ord('c'):
        figura_actual = "circulo"
        print("Figura: circulo")
    elif tecla == ord('l'):
        figura_actual = "linea"
        print("Figura: linea")
    elif tecla == ord('z'):
        if historial:
            lienzo = historial.pop()
            print("Deshacer realizado")
        else:
            print("No hay mas acciones para deshacer")
    elif tecla == ord('s'):
        cv2.imwrite("Imagenes/dibujo.jpg", lienzo)
        print("Dibujo guardado como dibujo.jpg")
    elif tecla == ord('q'):
        break

cv2.destroyAllWindows()