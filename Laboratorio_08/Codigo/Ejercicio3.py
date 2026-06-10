import cv2

# Abrir la imagen combinada creada en el ejercicio anterior
imagen_combinada = cv2.imread("Imagenes/combinada.jpg")

# Convertir a negativo (invertir valores de los pixeles)
negativo = 255 - imagen_combinada

# Guardar el negativo
cv2.imwrite("Imagenes/negativo.jpg", negativo)
print("Imagen negativo guardada correctamente")

# Mostrar el negativo
cv2.imshow("Negativo", negativo)
cv2.waitKey(0)
cv2.destroyAllWindows()

# Abrir el negativo guardado en escala de grises
gris = cv2.imread("Imagenes/negativo.jpg", cv2.IMREAD_GRAYSCALE)

# Guardar la imagen en escala de grises
cv2.imwrite("Imagenes/gris.jpg", gris)
print("Imagen en escala de grises guardada correctamente")

# Mostrar la imagen en escala de grises
cv2.imshow("Escala de Grises", gris)
cv2.waitKey(0)
cv2.destroyAllWindows()